using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Models
{
    public class UploadProxyRequest
    {
        public IFormFile File { get; set; } = null!;

        public string StudentName { get; set; } = string.Empty;

        public string AssignmentName { get; set; } = string.Empty;
    }
}
