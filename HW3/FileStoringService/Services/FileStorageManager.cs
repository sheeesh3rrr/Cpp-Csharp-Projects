using EDomainLib.Models;
using EDomainLib.Utils;
using FileStoringService.Data;
using FileStoringService.Entities;
using Microsoft.EntityFrameworkCore;
using FileStoringService.Mapping;

namespace FileStoringService.Services
{
    public class FileStorageManager : IFileStorageService
    {
        private readonly FileStoringDbContext _db;
        private readonly string _root;

        public FileStorageManager(FileStoringDbContext db, IConfiguration cfg)
        {
            _db = db;
            _root = Path.GetFullPath(cfg["Storage:UploadPath"] ?? "uploads");
            if (!Directory.Exists(_root))
            {
                _ = Directory.CreateDirectory(_root);
            }
        }

        public async Task<Work> SaveAsync(IFormFile file, string student, string assignment)
        {
            WorkEntity workEntity = new() 
            {
                Id = Guid.NewGuid(),
                StudentName = student,
                AssignmentName = assignment,
                UploadTimeUtc = DateTime.UtcNow,
                FileName = file.FileName
            };

            string folder = Path.Combine(_root, workEntity.Id.ToString());
            _ = Directory.CreateDirectory(folder);

            string filePath = Path.Combine(folder, file.FileName);

            using (FileStream fs = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            byte[] bytes = await File.ReadAllBytesAsync(filePath);
            string hash = HashHelper.Sha256Hex(bytes);

            workEntity.FilePath = filePath;
            workEntity.TextHash = hash;

            _ = _db.Works.Add(workEntity);
            _ = await _db.SaveChangesAsync();

            Work work = WorkEntityMappingExtensions.ToDomain(workEntity);
            return work;
        }

        public async Task<Work?> GetAsync(Guid id)
        {
            WorkEntity? entity = await _db.Works.FirstOrDefaultAsync(x => x.Id == id);
            return entity == null ? null : WorkEntityMappingExtensions.ToDomain(entity);
        }

        public async Task<IEnumerable<Work>> GetAllAsync()
        {
            return await _db.Works
                .Select(e => WorkEntityMappingExtensions.ToDomain(e))
                .ToListAsync();
        }
    }
}
