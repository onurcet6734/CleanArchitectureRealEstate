using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Common.Models.Auth;

public record UpdateProfileCommand(string Email, string FirstName, string LastName, string PhoneNumber) : IRequest<Result>;
