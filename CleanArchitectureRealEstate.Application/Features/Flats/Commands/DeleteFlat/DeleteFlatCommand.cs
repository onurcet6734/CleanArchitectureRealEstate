using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.DeleteFlat
{
    public class DeleteFlatCommand : IRequest<Result>
    {
        public int FlatId { get; set; }
    }
}