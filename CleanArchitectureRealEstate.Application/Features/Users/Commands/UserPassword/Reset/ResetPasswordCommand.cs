using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Reset;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<object>;