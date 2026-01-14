using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList
{
    public class GetFlatListQuery : IRequest<List<FlatDto>>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}