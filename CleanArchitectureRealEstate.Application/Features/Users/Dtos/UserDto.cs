using System;

namespace CleanArchitectureRealEstate.Application.Features.Users.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public DateTime Created { get; set; }
    }
}