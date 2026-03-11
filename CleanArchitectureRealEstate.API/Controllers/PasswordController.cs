using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models.Auth;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Forgot;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Reset;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.ValidateToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/password")]
    public class PasswordController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {

            var result = await _mediator.Send(command);
            var errorProperty = result.GetType().GetProperty("error");

            if (errorProperty != null)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = _mediator.Send(command);
            var errorProperty = result.GetType().GetProperty("error");

            if (errorProperty != null)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateTokenCommand command)
        {

            var result = _mediator.Send(command);
            var errorProperty = result.GetType().GetProperty("error");

            if (errorProperty != null)
                return BadRequest(result);
            return Ok(result);
        }
    }

}