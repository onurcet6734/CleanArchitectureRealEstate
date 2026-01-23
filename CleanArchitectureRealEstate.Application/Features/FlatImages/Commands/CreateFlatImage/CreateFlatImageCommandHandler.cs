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
        private readonly ICurrentUserService _currentUserService;


        public CreateFlatImageCommandHandler(
            IFlatImageRepository flatImageRepository,
            IFlatRepository flatRepository,
            ICurrentUserService currentUserService)
        {
            _flatImageRepository = flatImageRepository;
            _flatRepository = flatRepository;
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
            {
                throw new InvalidOperationException("Flat bulunamadı.");
            }

            //if (request.FlatId != _currentUserService.UserId) {
            //    throw new UnauthorizedAccessException("You can not post another user's flat!");
            //} // TODO : I m gonna compare other flats

            var entity = new FlatImage
            {
                Url = request.Url.Trim(),
                IsCover = request.IsCover,
                FlatId = request.FlatId
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
