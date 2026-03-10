using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Queries.GetList
{
    public class GetUserListQuery : IRequest<List<UserDto>>
    {
    }
}