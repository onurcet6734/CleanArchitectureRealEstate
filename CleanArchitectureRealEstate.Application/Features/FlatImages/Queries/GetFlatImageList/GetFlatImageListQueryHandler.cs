using AutoMapper;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetList
{
    public class GetFlatImageListQueryHandler
        : IRequestHandler<GetFlatImageListQuery, List<FlatImageDto>>
    {
        private readonly IFlatImageRepository _flatImageRepository;
        private readonly IMapper _mapper;


        public GetFlatImageListQueryHandler(IFlatImageRepository flatImageRepository , IMapper mapper)
        {
            _flatImageRepository = flatImageRepository;
            _mapper = mapper;

        }

        public async Task<List<FlatImageDto>> Handle(
            GetFlatImageListQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _flatImageRepository
                .GetFlatImagesWithFlatAsync(request , cancellationToken);

            return _mapper.Map<List<FlatImageDto>>(entities);


            //return entities.Select(x => new FlatImageDto
            //{
            //    Id = x.Id,
            //    Url = x.Url,
            //    IsCover = x.IsCover,
            //    Flat = new FlatDto
            //    {
            //        Id = x.Flat.Id,
            //        Title = x.Flat.Title,
            //        Description = x.Flat.Description,
            //        Price = x.Flat.Price,
            //        Currency = x.Flat.Currency,
            //        City = x.Flat.City,
            //        District = x.Flat.District,
            //        AddressLine = x.Flat.AddressLine,
            //        Type = x.Flat.Type.Value,
            //        Status = x.Flat.Status.Value,
            //        Created = x.Flat.Created,
            //        Updated = x.Flat.Updated
            //    }
            //}).ToList();
        }
    }
}
