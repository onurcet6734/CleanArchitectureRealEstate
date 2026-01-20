using MediatR;
using CleanArchitectureRealEstate.Application.Features.Users.Dtos;

namespace CleanArchitectureRealEstate.Application.Features.Users.Queries.GetById
{
    public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;
}