using EDomainLib.DTOs;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.Services
{
    public interface IGatewayService
    {
        Task<ReportDto> UploadAndAnalyzeAsync(IFormFile file, string studentName, string assignmentName, CancellationToken ct = default);

        Task<IEnumerable<ReportDto>> GetReportsForWorkAsync(Guid workId, CancellationToken ct = default);

        Task<WorkDto> GetWorkAsync(Guid workId, CancellationToken ct = default);

        Task<IEnumerable<ReportDto>> GetAllReportsAsync(CancellationToken ct = default);
    }
}
