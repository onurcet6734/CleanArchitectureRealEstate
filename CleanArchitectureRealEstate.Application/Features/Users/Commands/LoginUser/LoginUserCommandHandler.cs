using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand , object>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<object> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUserNameAsync(request.Username, cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result.Failure("Invalid username or password");
        }
        var token = _tokenService.GenerateTokenAsync(user);
        return new { accessToken = token.Result, fullName = user.FullName };

    }
}
