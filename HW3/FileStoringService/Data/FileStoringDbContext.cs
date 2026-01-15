using FileStoringService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Data
{
    public class FileStoringDbContext(DbContextOptions<FileStoringDbContext> options) : DbContext(options)
    {
        public DbSet<WorkEntity> Works => Set<WorkEntity>();
    }
}
