using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.UploadFlatImage;
using MediatR;

public class UploadImagesCommandHandler : IRequestHandler<UploadImageCommand, List<Dictionary<string, object>>>
{
    private readonly IFileStorageService _fileStorageService;

    public UploadImagesCommandHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<List<Dictionary<string, object>>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        if (request.Files is null || !request.Files.Any())
            throw new ArgumentException("En az bir resim yüklemelisiniz");

        var uploadedImages = new List<Dictionary<string, object>>();

        foreach (var file in request.Files)
        {
            if (file.Length > 0)
            {
                var fileName = await _fileStorageService.SaveFileAsync(file, cancellationToken);

                // DTO yerine direkt dictionary
                uploadedImages.Add(new Dictionary<string, object>
                {
                    ["fileName"] = fileName,
                    ["url"] = $"/flat-images/{fileName}",
                    ["size"] = file.Length
                });
            }
        }

        return uploadedImages;
    }
}