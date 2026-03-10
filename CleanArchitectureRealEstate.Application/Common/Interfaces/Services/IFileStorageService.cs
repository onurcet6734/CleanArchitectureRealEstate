using Microsoft.AspNetCore.Http;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        Task<string> SaveFileAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
    }
}
