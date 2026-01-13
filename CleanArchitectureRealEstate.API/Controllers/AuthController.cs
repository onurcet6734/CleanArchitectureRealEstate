using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models.Auth;
using CleanArchitectureRealEstate.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    // ---------------- REGISTER ----------------
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Created = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, HttpContext.RequestAborted);
        return Ok();
    }

    // ---------------- LOGIN ----------------
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userRepository.GetByUserNameAsync(
            request.Username,
            HttpContext.RequestAborted);

        if (user is null)
            return Unauthorized();

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return Unauthorized();

        var token = _tokenService.GenerateToken(user);
        return Ok(new { accessToken = token });
    }
}
