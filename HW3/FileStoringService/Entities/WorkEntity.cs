namespace FileStoringService.Entities
{
    public class WorkEntity
    {
        public Guid Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string AssignmentName { get; set; } = string.Empty;
        public DateTime UploadTimeUtc { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? TextHash { get; set; }
    }
}
