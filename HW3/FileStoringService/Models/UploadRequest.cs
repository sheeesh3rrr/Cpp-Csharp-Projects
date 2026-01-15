using Microsoft.AspNetCore.Http;

namespace FileStoringService.Models
{
    public class UploadRequest
    {
        [FromForm]
        public IFormFile File { get; set; } = null!;

        [FromForm]
        public string StudentName { get; set; } = string.Empty;

        [FromForm]
        public string AssignmentName { get; set; } = string.Empty;
    }
}
