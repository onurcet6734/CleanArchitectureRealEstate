using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using CleanArchitectureRealEstate.Domain.Entities;
using MediatR;


namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Commands.CreateFlatImage
{
    public class CreateFlatImageCommandHandler
    : IRequestHandler<CreateFlatImageCommand, FlatImageDto>
    {
        private readonly IFlatImageRepository _flatImageRepository;
        private readonly IFlatRepository _flatRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;


        public CreateFlatImageCommandHandler(
        IFlatImageRepository flatImageRepository,
        IFlatRepository flatRepository,
        IFileStorageService fileStorageService,
        ICurrentUserService currentUserService)
        {
            _flatImageRepository = flatImageRepository;
            _flatRepository = flatRepository;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
        }


        public async Task<FlatImageDto> Handle(
        CreateFlatImageCommand request,
        CancellationToken cancellationToken)
        {
            var flat = await _flatRepository.GetByIdAsync(
            request.FlatId,
            cancellationToken);


            if (flat is null)
                throw new InvalidOperationException("Flat bulunamadı.");


            // 🔥 DOSYA UPLOAD (Infrastructure’da çözülür)
            var imageUrl = await _fileStorageService
            .UploadAsync(request.Image, "flat-images");


            var entity = new FlatImage
            {
                FlatId = request.FlatId,
                Url = imageUrl,
                IsCover = request.IsCover
            };


            await _flatImageRepository.AddAsync(entity, cancellationToken);


            return new FlatImageDto
            {
                Id = entity.Id,
                Url = entity.Url,
                IsCover = entity.IsCover,
                Flat = new FlatDto
                {
                    Id = flat.Id,
                    Title = flat.Title,
                    Description = flat.Description,
                    Price = flat.Price,
                    Currency = flat.Currency,
                    City = flat.City,
                    District = flat.District,
                    AddressLine = flat.AddressLine,
                    Type = flat.Type.Value,
                    Status = flat.Status.Value,
                    Created = flat.Created,
                    Updated = flat.Updated
                }
            };
        }
    }
}