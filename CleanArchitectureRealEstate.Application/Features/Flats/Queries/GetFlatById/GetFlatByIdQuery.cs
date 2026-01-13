using MediatR;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatById
{
    public record GetFlatByIdQuery(int Id) : IRequest<FlatDto>;
}
