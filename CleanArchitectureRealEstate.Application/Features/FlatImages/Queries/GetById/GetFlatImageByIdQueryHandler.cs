using AutoMapper;
using MediatR;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings;

namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetById
{
    public class GetFlatImageByIdQueryHandler
        : IRequestHandler<GetFlatImageByIdQuery, FlatImageDto>
    {
        private readonly IFlatImageRepository _flatImageRepository;
        private readonly IMapper _mapper;

        public GetFlatImageByIdQueryHandler(
            IFlatImageRepository flatImageRepository,
            IMapper mapper)
        {
            _flatImageRepository = flatImageRepository;
            _mapper = mapper;
        }

        public async Task<FlatImageDto?> Handle(
            GetFlatImageByIdQuery request,
            CancellationToken cancellationToken)
        {
            var flatImage = await _flatImageRepository
                .GetByIdWithFlatAsync(request.Id, cancellationToken);

            if (flatImage is null)
                return null;

            return _mapper.Map<FlatImageDto>(flatImage);
        }
    }
}
