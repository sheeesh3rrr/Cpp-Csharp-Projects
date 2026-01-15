using FileStoringService.Services;
using Microsoft.AspNetCore.Mvc;
using EDomainLib.Models;
using EDomainLib.Mappings;
using EDomainLib.DTOs;

namespace FileStoringService.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileUploadController(IFileStorageService manager) : ControllerBase
    {
        private readonly IFileStorageService _manager = manager;

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            Work work = await _manager.SaveAsync(
                request.File!,
                request.StudentName,
                request.AssignmentName);

            return Ok(work);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetWork(Guid id)
        {
            Work? work = await _manager.GetAsync(id);
            return work == null ? NotFound() : Ok(work);
        }
    }
}
