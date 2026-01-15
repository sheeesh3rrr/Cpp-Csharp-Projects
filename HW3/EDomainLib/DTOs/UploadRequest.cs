using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace EDomainLib.DTOs
{
    public class UploadRequest
    {
        [JsonIgnore]
        public IFormFile? File { get; set; }

        [JsonPropertyName("studentName")]
        public string StudentName { get; set; } = string.Empty;

        [JsonPropertyName("assignmentName")]
        public string AssignmentName { get; set; } = string.Empty;
    }
}
