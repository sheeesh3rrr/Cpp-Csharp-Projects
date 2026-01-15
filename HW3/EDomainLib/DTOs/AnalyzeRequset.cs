namespace EDomainLib.DTOs
{
    public class AnalyzeRequest
    {
        public Guid WorkId { get; set; }

        public string? FilePath { get; set; }

        public string? FileName { get; set; }

        public string? AssignmentName { get; set; }

        public string? StudentName { get; set; }
    }
}
