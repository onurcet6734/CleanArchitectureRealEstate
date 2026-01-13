using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList
{
    public class GetFlatListQuery : IRequest<List<FlatDto>>
    {
    }
}