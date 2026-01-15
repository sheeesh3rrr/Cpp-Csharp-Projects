using EDomainLib.DTOs;

namespace FileAnalysisService.Services
{
    public interface IFileAnalysisService
    {
        Task<ReportDto> AnalyzeAsync(WorkDto workDto);

        Task<ReportDto?> GetReportByWorkIdAsync(Guid workId);

        Task<IEnumerable<ReportDto>> GetAllReportsAsync();
    }
}
