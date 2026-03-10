using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    int UserId,
    string OldPassword,
    string NewPassword
) : IRequest<Result>;
