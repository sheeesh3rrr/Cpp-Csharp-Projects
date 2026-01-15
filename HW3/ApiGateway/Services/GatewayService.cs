using EDomainLib.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Services
{
    public class GatewayService(IHttpClientFactory httpFactory, ILogger<GatewayService> logger) : IGatewayService
    {
        private readonly IHttpClientFactory _httpFactory = httpFactory;
        private readonly ILogger<GatewayService> _logger = logger;

        public async Task<ReportDto> UploadAndAnalyzeAsync(
            IFormFile file,
            string studentName,
            string assignmentName,
            CancellationToken ct = default)
        {

            HttpClient storingClient = _httpFactory.CreateClient("filestoring");

            using MultipartFormDataContent multipart = [];

            using MemoryStream ms = new();
            await file.CopyToAsync(ms, ct);
            _ = ms.Seek(0, SeekOrigin.Begin);

            ByteArrayContent fileContent = new(ms.ToArray());
            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");

            multipart.Add(fileContent, "file", file.FileName);
            multipart.Add(new StringContent(studentName), "studentName");
            multipart.Add(new StringContent(assignmentName), "assignmentName");

            HttpResponseMessage storeResp;

            try
            {
                storeResp = await storingClient.PostAsync("files/upload", multipart, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FileStoring request failed");
                throw new InvalidOperationException("Error calling FileStoringService");
            }

            if (!storeResp.IsSuccessStatusCode)
            {
                string err = await storeResp.Content.ReadAsStringAsync(ct);
                _logger.LogError("FileStoring returned {Status}: {Body}", storeResp.StatusCode, err);
                throw new InvalidOperationException($"FileStoringService error: {storeResp.StatusCode}");
            }

            string storeJson = await storeResp.Content.ReadAsStringAsync(ct);

            WorkDto workDto = JsonSerializer.Deserialize<WorkDto>(storeJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Invalid WorkDto from FileStoringService");

            HttpClient analysisClient = _httpFactory.CreateClient("fileanalysis");

            StringContent jsonPayload = new(
                JsonSerializer.Serialize(workDto),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage analysisResp;

            try
            {
                analysisResp = await analysisClient.PostAsync("reports/analyze", jsonPayload, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FileAnalysis request failed");
                throw new InvalidOperationException("Error calling FileAnalysisService");
            }

            if (!analysisResp.IsSuccessStatusCode)
            {
                string err = await analysisResp.Content.ReadAsStringAsync(ct);
                _logger.LogError("FileAnalysis returned {Status}: {Body}", analysisResp.StatusCode, err);
                throw new InvalidOperationException($"FileAnalysisService error: {analysisResp.StatusCode}");
            }

            string analysisJson = await analysisResp.Content.ReadAsStringAsync(ct);

            ReportDto reportDto = JsonSerializer.Deserialize<ReportDto>(analysisJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Invalid ReportDto from FileAnalysisService");

            return reportDto;
        }

        public async Task<IEnumerable<ReportDto>> GetReportsForWorkAsync(Guid workId, CancellationToken ct = default)
        {
            HttpClient client = _httpFactory.CreateClient("fileanalysis");

            HttpResponseMessage resp =
                await client.GetAsync($"reports/work/{workId}", ct);

            if (!resp.IsSuccessStatusCode)
            {
                string err = await resp.Content.ReadAsStringAsync(ct);
                _logger.LogError("GetReports failed: {Status} {Body}",
                    resp.StatusCode, err);
                throw new InvalidOperationException(
                    $"FileAnalysisService returned {resp.StatusCode}");
            }

            string json = await resp.Content.ReadAsStringAsync(ct);

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                List<ReportDto>? list = JsonSerializer.Deserialize<List<ReportDto>>(json, options);
                if (list != null)
                {
                    return list;
                }
            }
            catch {}

            try
            {
                ReportDto? single = JsonSerializer.Deserialize<ReportDto>(json, options);
                if (single != null)
                {
                    return [single];
                }
            }
            catch {}

            throw new InvalidOperationException("Unsupported response format from FileAnalysisService");
        }

        public async Task<WorkDto> GetWorkAsync(Guid workId, CancellationToken ct = default)
        {
            HttpClient client = _httpFactory.CreateClient("filestoring");

            HttpResponseMessage resp;
            try
            {
                resp = await client.GetAsync($"files/{workId}", ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FileStoring request failed");
                throw new InvalidOperationException("Error calling FileStoringService");
            }

            if (!resp.IsSuccessStatusCode)
            {
                string err = await resp.Content.ReadAsStringAsync(ct);
                _logger.LogError("FileStoring returned {Status}: {Body}",
                    resp.StatusCode, err);
                throw new InvalidOperationException(
                    $"FileStoringService returned {resp.StatusCode}");
            }

            string json = await resp.Content.ReadAsStringAsync(ct);

            WorkDto work = JsonSerializer.Deserialize<WorkDto>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException(
                    "Invalid WorkDto from FileStoringService");

            return work;
        }

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync(CancellationToken ct = default)
        {
            HttpClient client = _httpFactory.CreateClient("fileanalysis");

            HttpResponseMessage resp;
            try
            {
                resp = await client.GetAsync("reports", ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FileAnalysis request failed");
                throw new InvalidOperationException("Error calling FileAnalysisService");
            }

            if (!resp.IsSuccessStatusCode)
            {
                string err = await resp.Content.ReadAsStringAsync(ct);
                _logger.LogError(
                    "GetAllReports failed: {Status} {Body}",
                    resp.StatusCode, err);

                throw new InvalidOperationException(
                    $"FileAnalysisService returned {resp.StatusCode}");
            }

            string json = await resp.Content.ReadAsStringAsync(ct);

            List<ReportDto>? reports =
                JsonSerializer.Deserialize<List<ReportDto>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return reports
                ?? throw new InvalidOperationException(
                    "Invalid response from FileAnalysisService");
        }


    }
}
