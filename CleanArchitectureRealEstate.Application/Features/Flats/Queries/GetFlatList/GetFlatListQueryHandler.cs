using AutoMapper;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList
{
    public class GetFlatListQueryHandler
        : IRequestHandler<GetFlatListQuery, List<FlatDto>>
    {
        private readonly IFlatRepository _flatRepository;
        private readonly IMapper _mapper;

        public GetFlatListQueryHandler(
            IFlatRepository flatRepository,
            IMapper mapper)
        {
            _flatRepository = flatRepository;
            _mapper = mapper;
        }

        public async Task<List<FlatDto>> Handle(
            GetFlatListQuery request,
            CancellationToken cancellationToken)
        {
            var flats = await _flatRepository.GetAllAsync(request.Page , request.Limit , cancellationToken);
            return _mapper.Map<List<FlatDto>>(flats);
        }
    }
}
