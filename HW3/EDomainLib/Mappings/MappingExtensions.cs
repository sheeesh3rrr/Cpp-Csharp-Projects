using EDomainLib.Models;
using EDomainLib.DTOs;

namespace EDomainLib.Mappings
{
    public static class MappingExtensions
    {
        public static WorkDto ToDto(this Work w)
        {
            return w == null ? throw new ArgumentNullException(nameof(w)) : 
                new(w.Id, w.StudentName, w.AssignmentName, w.UploadTimeUtc, w.FileName, w.FilePath, w.TextHash);
        }

        public static Work ToDomain(this WorkDto d)
        {
            return d == null ? throw new ArgumentNullException(nameof(d)) :
                new(d.Id, d.StudentName, d.AssignmentName, d.UploadTimeUtc, d.FileName, d.FilePath, d.TextHash);
        }

        public static ReportDto ToDto(this Report r)
        {
            return r == null ? throw new ArgumentNullException(nameof(r)) :
                new(r.Id, r.WorkId, r.Status, r.IsPlagiarism, r.Summary, r.ReportFilePath, r.CreatedAtUtc, r.DetailsJson);
        }

        public static Report ToDomain(this ReportDto d)
        {
            return d == null ? throw new ArgumentNullException(nameof(d)) :
                new(d.Id, d.WorkId, d.Status, d.IsPlagiarism, d.Summary, d.ReportFilePath, d.CreatedAtUtc, d.DetailsJson);
        }
    }
}
