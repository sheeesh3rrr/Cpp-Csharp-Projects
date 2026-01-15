using EDomainLib.DTOs;
using EDomainLib.Utils;
using FileAnalysisService.Data;
using FileAnalysisService.Entities;
using FileAnalysisService.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FileAnalysisService.Services
{
    public class FileAnalysisManager : IFileAnalysisService
    {
        private readonly FileAnalysisDbContext _db;
        private readonly string _reportsRoot;
        private readonly ILogger<FileAnalysisManager> _logger;
        private readonly IHttpClientFactory _httpFactory;

        public FileAnalysisManager(FileAnalysisDbContext db, IConfiguration cfg, IWebHostEnvironment env, ILogger<FileAnalysisManager> logger, IHttpClientFactory httpFactory)
        {
            _db = db;
            _logger = logger;
            _httpFactory = httpFactory;

            string? configured = cfg["Storage:ReportsPath"];
            if (string.IsNullOrWhiteSpace(configured))
            {
                _reportsRoot = Path.Combine(env.ContentRootPath, "reports");
                _logger.LogInformation("ReportsPath not configured, using default: {Path}", _reportsRoot);
            }
            else
            {
                _reportsRoot = Path.IsPathRooted(configured) ? Path.GetFullPath(configured) : Path.GetFullPath(Path.Combine(env.ContentRootPath, configured));
            }

            if (!Directory.Exists(_reportsRoot))
            {
                _ = Directory.CreateDirectory(_reportsRoot);
            }
        }

        public async Task<ReportDto> AnalyzeAsync(WorkDto workDto)
        {
            if (workDto == null)
            {
                throw new ArgumentNullException(nameof(workDto));
            }

            ReportEntity reportEntity = new()
            {
                WorkId = workDto.Id,
                CreatedAtUtc = DateTime.UtcNow,
                Status = (int)EDomainLib.Enums.ReportStatus.Processing
            };

            if (string.IsNullOrWhiteSpace(workDto.FilePath))
            {
                reportEntity.Status = (int)EDomainLib.Enums.ReportStatus.Failed;
                reportEntity.Summary = "FilePath not provided in WorkDto.";
                await SaveReportEntityAsync(reportEntity);
                return ReportEntityMappingExtensions.ToDto(reportEntity);
            }

            if (!File.Exists(workDto.FilePath))
            {
                reportEntity.Status = (int)EDomainLib.Enums.ReportStatus.Failed;
                reportEntity.Summary = $"File not found at path: {workDto.FilePath}";
                await SaveReportEntityAsync(reportEntity);
                return ReportEntityMappingExtensions.ToDto(reportEntity);
            }

            string ext = Path.GetExtension(workDto.FilePath).ToLowerInvariant();
            if (ext != ".txt")
            {
                reportEntity.Status = (int)EDomainLib.Enums.ReportStatus.Failed;
                reportEntity.Summary = "Unsupported file type. Only .txt files supported.";
                await SaveReportEntityAsync(reportEntity);
                return ReportEntityMappingExtensions.ToDto(reportEntity);
            }

            string text;
            try
            {
                text = await File.ReadAllTextAsync(workDto.FilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read file {Path}", workDto.FilePath);
                reportEntity.Status = (int)EDomainLib.Enums.ReportStatus.Failed;
                reportEntity.Summary = "Failed to read file.";
                await SaveReportEntityAsync(reportEntity);
                return ReportEntityMappingExtensions.ToDto(reportEntity);
            }

            string textHash = HashHelper.Sha256Hex(text);
            reportEntity.TextHash = textHash;

            bool plagiarismFound = await _db.Reports.AnyAsync(r => r.TextHash == textHash && r.WorkId != workDto.Id);

            reportEntity.IsPlagiarism = plagiarismFound;
            reportEntity.Status = (int)EDomainLib.Enums.ReportStatus.Completed;
            reportEntity.Summary = plagiarismFound ? "Plagiarism detected (matching earlier submission)." : "No plagiarism detected.";

            var details = new
            {
                WordCount = CountWords(text),
                CharCount = text.Length,
                TextHash = textHash,
                workDto.StudentName,
                workDto.AssignmentName
            };

            reportEntity.DetailsJson = JsonSerializer.Serialize(details);

            string reportFileName = $"{reportEntity.Id}.report.json";
            string reportFilePath = Path.Combine(_reportsRoot, reportFileName);
            var reportOutput = new
            {
                ReportId = reportEntity.Id,
                reportEntity.WorkId,
                reportEntity.IsPlagiarism,
                reportEntity.Summary,
                Details = details,
                reportEntity.CreatedAtUtc
            };

            try
            {
                await File.WriteAllTextAsync(reportFilePath, JsonSerializer.Serialize(reportOutput, new JsonSerializerOptions { WriteIndented = true }));
                reportEntity.ReportFilePath = reportFilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write report file {Path}", reportFilePath);
                reportEntity.ReportFilePath = null;
            }

            await GenerateWordCloudAsync(text, reportEntity.Id);
            await SaveReportEntityAsync(reportEntity);

            return ReportEntityMappingExtensions.ToDto(reportEntity);
        }

        private static int CountWords(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return 0;
            }

            string[] parts = s.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length;
        }

        private async Task SaveReportEntityAsync(ReportEntity entity)
        {
            bool exists = await _db.Reports.AnyAsync(r => r.Id == entity.Id);
            _ = !exists ? _db.Reports.Add(entity) : _db.Reports.Update(entity);
            _ = await _db.SaveChangesAsync();
        }

        public async Task<ReportDto?> GetReportByWorkIdAsync(Guid workId)
        {
            ReportEntity? ent = await _db.Reports.OrderByDescending(r => r.CreatedAtUtc).FirstOrDefaultAsync(r => r.WorkId == workId);
            return ent == null ? null : ReportEntityMappingExtensions.ToDto(ent); ;
        }

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
        {
            return await _db.Reports.OrderByDescending(r => r.CreatedAtUtc).Select(r => r.ToDto()).ToListAsync();
        }

        private async Task GenerateWordCloudAsync(string text, Guid reportId)
        {
            var words = text
                .Split(new[] { ' ', '\r', '\n', '\t', '.', ',', ';', ':', '-', '(', ')', '[', ']' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLowerInvariant())
                .Where(w => w.Length > 2)
                .GroupBy(w => w)
                .Select(g => new { text = g.Key, weight = g.Count() })
                .OrderByDescending(x => x.weight)
                .Take(100)
                .ToArray();

            if (!words.Any())
            {
                return;
            }

            var payload = new { words };
            HttpClient client = _httpFactory.CreateClient();

            HttpRequestMessage request = new(HttpMethod.Post, "https://quickchart.io/wordcloud")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json")
            };

            HttpResponseMessage resp = await client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
            {
                return;
            }

            byte[] imageBytes = await resp.Content.ReadAsByteArrayAsync();
            string pngPath = Path.Combine(_reportsRoot, $"{reportId}.wordcloud.png");
            await File.WriteAllBytesAsync(pngPath, imageBytes);
        }
    }
}
