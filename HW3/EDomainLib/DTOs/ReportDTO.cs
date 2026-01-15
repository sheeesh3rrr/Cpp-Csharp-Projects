using System.Text.Json.Serialization;
using EDomainLib.Enums;

namespace EDomainLib.DTOs
{
    public class ReportDto(Guid id, Guid workId, ReportStatus status, bool isPlagiarism, string summary, string? reportFilePath, DateTime createdAtUtc, string? detailsJson)
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = id;

        [JsonPropertyName("workId")]
        public Guid WorkId { get; set; } = workId;

        [JsonPropertyName("status")]
        public ReportStatus Status { get; set; } = status;

        [JsonPropertyName("isPlagiarism")]
        public bool IsPlagiarism { get; set; } = isPlagiarism;

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = summary;

        [JsonPropertyName("reportFilePath")]
        public string? ReportFilePath { get; set; } = reportFilePath;

        [JsonPropertyName("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; } = createdAtUtc;

        [JsonPropertyName("detailsJson")]
        public string? DetailsJson { get; set; } = detailsJson;
    }
}
