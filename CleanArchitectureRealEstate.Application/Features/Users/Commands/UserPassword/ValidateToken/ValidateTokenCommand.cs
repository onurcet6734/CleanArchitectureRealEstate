using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.ValidateToken;

public record ValidateTokenCommand(string Token) : IRequest<object>;