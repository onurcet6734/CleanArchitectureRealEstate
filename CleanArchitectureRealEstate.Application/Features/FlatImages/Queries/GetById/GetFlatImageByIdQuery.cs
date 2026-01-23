using CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetById
{
    public record GetFlatImageByIdQuery(int Id)
        : IRequest<FlatImageDto?>;
}
