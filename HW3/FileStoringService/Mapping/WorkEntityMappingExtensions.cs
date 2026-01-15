using FileStoringService.Entities;
using EDomainLib.Models;

namespace FileStoringService.Mapping
{
    public static class WorkEntityMappingExtensions
    {
        public static Work ToDomain(this WorkEntity e)
        {
            return new(e.Id, e.StudentName, e.AssignmentName, e.UploadTimeUtc, e.FileName, e.FilePath, e.TextHash);
        }

        public static WorkEntity ToEntity(this Work w)
        {
            return new WorkEntity
            {
                Id = w.Id,
                StudentName = w.StudentName,
                AssignmentName = w.AssignmentName,
                UploadTimeUtc = w.UploadTimeUtc,
                FileName = w.FileName,
                FilePath = w.FilePath,
                TextHash = w.TextHash
            };
        }
    }
}
