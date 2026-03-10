using AutoMapper;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Queries.GetById
{
    public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, ProfileDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;   

        public GetProfileByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<ProfileDto?> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            return user is null ? null : _mapper.Map<ProfileDto>(user);
        }
    }
}
