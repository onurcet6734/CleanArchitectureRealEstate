using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CleanArchitectureRealEstate.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        
        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            var uploadsPath = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folder}/{fileName}";
        }

        public async Task<string> SaveFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var folder = "flat-images";
            var uploadsPath = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return fileName;
        }

        public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                // fileUrl format: "/flat-images/guid.jpg" veya "guid.jpg"
                var fileName = Path.GetFileName(fileUrl);
                var folder = "flat-images";
                var fullPath = Path.Combine(_env.WebRootPath, folder, fileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception)
            {
                // Log error but don't throw
            }

            return Task.CompletedTask;
        }
    }
}
