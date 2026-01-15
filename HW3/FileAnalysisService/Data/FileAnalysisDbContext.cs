using FileAnalysisService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Data
{
    public class FileAnalysisDbContext(DbContextOptions<FileAnalysisDbContext> options) : DbContext(options)
    {
        public DbSet<ReportEntity> Reports => Set<ReportEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<ReportEntity>(eb =>
            {
                _ = eb.HasKey(r => r.Id);
                _ = eb.Property(r => r.Summary).HasMaxLength(2000);
                _ = eb.Property(r => r.DetailsJson).HasColumnType("TEXT");
                _ = eb.Property(r => r.ReportFilePath).HasMaxLength(1000);
            });
        }
    }
}
