using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.FlatImagess.Commands.UpdateFlatImage
{
    public record UpdateFlatImageCommand(
        int Id,
        string? Url,
        bool? IsCover
    ) : IRequest<Result>;
}