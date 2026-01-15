namespace FileAnalysisService.Entities
{
    public class ReportEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid WorkId { get; set; }

        public string? TextHash { get; set; }

        public bool IsPlagiarism { get; set; }

        public int Status { get; set; }

        public string Summary { get; set; } = string.Empty;

        public string? ReportFilePath { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public string? DetailsJson { get; set; }
    }
}
