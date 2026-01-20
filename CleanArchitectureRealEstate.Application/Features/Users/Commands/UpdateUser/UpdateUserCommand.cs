using MediatR;
using CleanArchitectureRealEstate.Application.Common.Models;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result>
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

}