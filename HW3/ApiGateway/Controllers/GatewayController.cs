using ApiGateway.Models;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api")]
    public class GatewayController(IGatewayService service, ILogger<GatewayController> logger) : ControllerBase
    {
        private readonly IGatewayService _service = service;
        private readonly ILogger<GatewayController> _logger = logger;

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadProxyRequest request)
        {
            if (request.File == null)
            {
                return BadRequest("File is required.");
            }

            try
            {
                EDomainLib.DTOs.ReportDto report = await _service.UploadAndAnalyzeAsync(
                    request.File,
                    request.StudentName,
                    request.AssignmentName
                );

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload failed.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("works/{workId}/reports")]
        public async Task<IActionResult> GetReports(Guid workId)
        {
            try
            {
                IEnumerable<EDomainLib.DTOs.ReportDto> list = await _service.GetReportsForWorkAsync(workId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get reports for work {workId}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("works/{workId}")]
        public async Task<IActionResult> GetWork(Guid workId)
        {
            try
            {
                EDomainLib.DTOs.WorkDto work = await _service.GetWorkAsync(workId);
                return Ok(work);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get work {workId}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                IEnumerable<EDomainLib.DTOs.ReportDto> list = await _service.GetAllReportsAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all reports");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
