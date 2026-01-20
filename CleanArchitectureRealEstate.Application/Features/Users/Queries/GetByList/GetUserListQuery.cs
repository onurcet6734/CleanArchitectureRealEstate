using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using MediatR;
using System.Collections;
using System.Collections.Generic;

namespace CleanArchitectureRealEstate.Application.Features.Users.Queries.GetList
{
    public class GetUserListQuery : IRequest<List<UserDto>>
    {
    }
}