using EDomainLib.Models;
using Microsoft.AspNetCore.Http;

namespace FileStoringService.Services
{
    public interface IFileStorageService
    {
        Task<Work> SaveAsync(IFormFile file, string student, string assignment);
        Task<Work?> GetAsync(Guid id);
        Task<IEnumerable<Work>> GetAllAsync();
    }
}
