namespace CleanArchitectureRealEstate.Application.Features.Users.Dtos;

public record ProfileDto(
    int Id,
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    bool IsEDevletVerified,
    DateTime? EDevletVerifiedAt,
    DateTime Created
);
