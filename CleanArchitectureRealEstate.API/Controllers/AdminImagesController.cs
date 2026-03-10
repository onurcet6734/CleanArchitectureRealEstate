using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin/images")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminImagesController : ControllerBase
    {
        private readonly IFlatImageRepository _imageRepository;
        private readonly IFlatRepository _flatRepository;
        private readonly IFileStorageService _fileStorageService;

        public AdminImagesController(
            IFlatImageRepository imageRepository,
            IFlatRepository flatRepository,
            IFileStorageService fileStorageService)
        {
            _imageRepository = imageRepository;
            _flatRepository = flatRepository;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAllAsync(HttpContext.RequestAborted);
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var image = await _imageRepository.GetByIdAsync(id, HttpContext.RequestAborted);
            if (image is null)
                return NotFound(new { error = "Image not found" });

            return Ok(image);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            if (request.File is null || request.File.Length == 0)
                return BadRequest(new { error = "File is required" });

            // Validate flat exists
            var flat = await _flatRepository.GetByIdAsync(request.FlatId, HttpContext.RequestAborted);
            if (flat is null)
                return NotFound(new { error = "Flat not found" });

            // Save file
            var fileName = await _fileStorageService.SaveFileAsync(request.File, HttpContext.RequestAborted);

            // Create image entity
            var image = new FlatImage
            {
                FlatId = request.FlatId,
                ImageUrl = $"/flat-images/{fileName}",
                Url = $"/flat-images/{fileName}", // Url ve ImageUrl'yi senkronize et
                IsPrimary = request.IsPrimary,
                IsCover = request.IsPrimary, // IsPrimary ve IsCover'ı senkronize et
                Created = DateTime.UtcNow
            };

            await _imageRepository.AddAsync(image, HttpContext.RequestAborted);

            return CreatedAtAction(nameof(GetImageById), new { id = image.Id }, image);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromForm] UpdateImageRequest request)
        {
            var image = await _imageRepository.GetByIdAsync(id, HttpContext.RequestAborted);
            if (image is null)
                return NotFound(new { error = "Image not found" });

            // If new file provided, replace old one
            if (request.File is not null && request.File.Length > 0)
            {
                // Delete old file
                await _fileStorageService.DeleteFileAsync(image.ImageUrl, HttpContext.RequestAborted);

                // Save new file
                var fileName = await _fileStorageService.SaveFileAsync(request.File, HttpContext.RequestAborted);
                image.ImageUrl = $"/flat-images/{fileName}";
                image.Url = $"/flat-images/{fileName}"; // Url ve ImageUrl'yi senkronize et
            }

            if (request.IsPrimary.HasValue)
            {
                image.IsPrimary = request.IsPrimary.Value;
                image.IsCover = request.IsPrimary.Value; // IsPrimary ve IsCover'ı senkronize et
            }

            image.Updated = DateTime.UtcNow;
            await _imageRepository.UpdateAsync(image, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _imageRepository.GetByIdAsync(id, HttpContext.RequestAborted);
            if (image is null)
                return NotFound(new { error = "Image not found" });

            // Delete file from storage
            await _fileStorageService.DeleteFileAsync(image.ImageUrl, HttpContext.RequestAborted);

            // Delete from database (soft delete)
            image.IsDeleted = true;
            image.Updated = DateTime.UtcNow;
            await _imageRepository.UpdateAsync(image, HttpContext.RequestAborted);

            return NoContent();
        }
    }

    public record UploadImageRequest(int FlatId, IFormFile? File, bool IsPrimary = false);
    public record UpdateImageRequest(IFormFile? File, bool? IsPrimary);
}
