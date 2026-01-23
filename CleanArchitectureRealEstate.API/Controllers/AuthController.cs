using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using CleanArchitectureRealEstate.Application.Common.Models.Auth;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.CreateUser;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.UpdateUser;
using CleanArchitectureRealEstate.Application.Features.Users.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(
            IMediator mediator,
            IUserRepository userRepository,
            ITokenService tokenService,
            IPasswordHasher passwordHasher)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new CreateUserCommand(
                request.Username,
                request.Email,
                request.Password
            );

            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = result.Id },
                result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetByUserNameAsync(
                request.Username,
                HttpContext.RequestAborted);

            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { error = "Invalid username or password" });
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { accessToken = token });
        }

        [Authorize]
        [HttpPatch("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new { error = result.Error });
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result is null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(result);
        }
    }
}