using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Queries.GetById;

public record GetProfileByIdQuery(int Id) : IRequest<ProfileDto?>
{
}
