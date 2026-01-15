namespace EDomainLib.Models
{
    public class Work(Guid id, string studentName, string assignmentName, DateTime uploadTimeUtc, string fileName, string filePath, string? textHash)
    {
        public Guid Id { get; set; } = id;

        public string StudentName { get; set; } = studentName;

        public string AssignmentName { get; set; } = assignmentName;

        public DateTime UploadTimeUtc { get; set; } = uploadTimeUtc;

        public string FileName { get; set; } = fileName;

        public string FilePath { get; set; } = filePath;

        public string? TextHash { get; set; } = textHash;
    }
}
