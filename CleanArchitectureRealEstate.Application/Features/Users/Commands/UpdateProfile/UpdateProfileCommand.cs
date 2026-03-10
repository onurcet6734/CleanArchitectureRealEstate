using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

public record UpdateProfileCommand(string Email, string FirstName, string LastName, string PhoneNumber) : IRequest<Result>;