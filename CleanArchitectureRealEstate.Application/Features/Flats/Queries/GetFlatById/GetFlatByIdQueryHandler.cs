using AutoMapper;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Domain.Exceptions;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatById
{
    public class GetFlatByIdQueryHandler
        : IRequestHandler<GetFlatByIdQuery, FlatDto>
    {
        private readonly IFlatRepository _flatRepository;
        private readonly IMapper _mapper;

        public GetFlatByIdQueryHandler(
            IFlatRepository flatRepository,
            IMapper mapper)
        {
            _flatRepository = flatRepository;
            _mapper = mapper;
        }

        public async Task<FlatDto> Handle(
            GetFlatByIdQuery request,
            CancellationToken cancellationToken)
        {
            var flat = await _flatRepository.GetByIdAsync(request.Id, cancellationToken);

            if (flat is null)
                throw new NotFoundException(nameof(Flat), request.Id);

            return _mapper.Map<FlatDto>(flat);
        }
    }
}
