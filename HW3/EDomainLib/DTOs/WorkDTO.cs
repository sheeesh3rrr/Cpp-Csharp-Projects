using System.Text.Json.Serialization;

namespace EDomainLib.DTOs
{
    public class WorkDto(Guid id, string studentName, string assignmentName, DateTime uploadTimeUtc, string fileName, string filePath, string? textHash)
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = id;

        [JsonPropertyName("studentName")]
        public string StudentName { get; set; } = studentName;

        [JsonPropertyName("assignmentName")]
        public string AssignmentName { get; set; } = assignmentName;

        [JsonPropertyName("uploadTimeUtc")]
        public DateTime UploadTimeUtc { get; set; } = uploadTimeUtc;

        [JsonPropertyName("fileName")]
        public string FileName { get; set; } = fileName;

        [JsonPropertyName("filePath")]
        public string FilePath { get; set; } = filePath;

        [JsonPropertyName("textHash")]
        public string? TextHash { get; set; } = textHash;
    }
}
