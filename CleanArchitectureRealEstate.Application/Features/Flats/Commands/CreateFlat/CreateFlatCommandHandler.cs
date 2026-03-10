using AutoMapper;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Domain.Exceptions;
using CleanArchitectureRealEstate.Domain.ValueObjects;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.CreateFlat
{
    public class CreateFlatCommandHandler
     : IRequestHandler<CreateFlatCommand, FlatDto>
    {
        private readonly IFlatRepository _flatRepository;
        private readonly IFlatImageRepository _flatImageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateFlatCommandHandler(
            IFlatRepository flatRepository,
            IFlatImageRepository flatImageRepository,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _flatRepository = flatRepository;
            _flatImageRepository = flatImageRepository;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<FlatDto> Handle(CreateFlatCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId;            
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                throw new NotFoundException(nameof(User), userId);

            var flat = new Flat
            {
                UserId = userId,
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Currency = request.Currency,
                City = request.City,
                District = request.District,
                AddressLine = request.AddressLine,
                Type = FlatType.From(request.Type),
                Status = FlatStatus.From(request.Status),
                Created = DateTime.UtcNow
            };

            await _flatRepository.AddAsync(flat, cancellationToken);

            if (request.ImageUrls is not null && request.ImageUrls.Any())
            {
                for (int i = 0; i < request.ImageUrls.Count; i++)
                {
                    var image = new FlatImage
                    {
                        FlatId = flat.Id,
                        ImageUrl = request.ImageUrls[i],
                        Url = request.ImageUrls[i],
                        IsPrimary = i == 0,
                        IsCover = i == 0,
                        Created = DateTime.UtcNow
                    };

                    await _flatImageRepository.AddAsync(image, cancellationToken);
                }
            }

            return _mapper.Map<FlatDto>(flat);
        }
    }
}
