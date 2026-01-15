using EDomainLib.DTOs;
using FileAnalysisService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysisService.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportController(IFileAnalysisService analysis) : ControllerBase
    {
        private readonly IFileAnalysisService _analysis = analysis;

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] WorkDto workDto)
        {
            if (workDto == null)
            {
                return BadRequest("WorkDto is required.");
            }

            ReportDto report = await _analysis.AnalyzeAsync(workDto);
            return Ok(report);
        }

        [HttpGet("work/{workId}")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetByWorkId(Guid workId)
        {
            ReportDto? r = await _analysis.GetReportByWorkIdAsync(workId);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ReportDto> list = await _analysis.GetAllReportsAsync();
            return Ok(list);
        }
    }
}
