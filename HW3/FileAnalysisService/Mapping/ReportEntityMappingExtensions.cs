using FileAnalysisService.Entities;
using EDomainLib.Models;
using EDomainLib.DTOs;
using EDomainLib.Enums;

namespace FileAnalysisService.Mapping
{
    public static class ReportEntityMappingExtensions
    {
        public static Report ToDomain(this ReportEntity e)
        {
            return new(e.Id, e.WorkId, (ReportStatus)e.Status, e.IsPlagiarism, e.Summary, e.ReportFilePath, e.CreatedAtUtc, e.DetailsJson);
        }

        public static ReportEntity ToEntity(this Report r)
        {
            return new ReportEntity
            {
                Id = r.Id,
                WorkId = r.WorkId,
                Status = (int)r.Status,
                IsPlagiarism = r.IsPlagiarism,
                Summary = r.Summary,
                ReportFilePath = r.ReportFilePath,
                CreatedAtUtc = r.CreatedAtUtc,
                DetailsJson = r.DetailsJson
            };
        }

        public static ReportDto ToDto(this ReportEntity e)
        {
            return new(e.Id, e.WorkId, (ReportStatus)e.Status, e.IsPlagiarism, e.Summary, e.ReportFilePath, e.CreatedAtUtc, e.DetailsJson);
        }
    }
}
