using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Forgot;

public record ForgotPasswordCommand(string Email) : IRequest<object>;
