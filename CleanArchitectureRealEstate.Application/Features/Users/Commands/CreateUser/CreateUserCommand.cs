using MediatR;
using CleanArchitectureRealEstate.Application.Features.Users.Dtos;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserCommand(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }

        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
