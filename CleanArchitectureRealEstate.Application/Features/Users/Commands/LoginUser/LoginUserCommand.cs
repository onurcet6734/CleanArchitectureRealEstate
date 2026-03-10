using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.LoginUser;

public record LoginUserCommand(string Username , string Password) : IRequest<object>;
