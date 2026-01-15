using EDomainLib.Enums;

namespace EDomainLib.Models
{
    public class Report(Guid id, Guid workId, ReportStatus status, bool isPlagiarism, string summary, string? reportFilePath, DateTime createdAtUtc, string? detailsJson)
    {
        public Guid Id { get; set; } = id;

        public Guid WorkId { get; set; } = workId;

        public ReportStatus Status { get; set; } = status;

        public bool IsPlagiarism { get; set; } = isPlagiarism;

        public string Summary { get; set; } = summary;

        public string? ReportFilePath { get; set; } = reportFilePath;

        public DateTime CreatedAtUtc { get; set; } = createdAtUtc;

        public string? DetailsJson { get; set; } = detailsJson;
    }
}
