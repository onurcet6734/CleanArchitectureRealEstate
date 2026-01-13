using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.DeleteFlat
{
    public record DeleteFlatCommand(int Id) : IRequest<Result>;
}
