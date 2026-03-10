using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserCommand(string username, string email, string password , string phoneNumber, string firstName , string lastName)
        {
            Username = username;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
        }

        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string PhoneNumber { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
    }
}
