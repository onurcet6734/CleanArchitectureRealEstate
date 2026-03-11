using CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Forgot;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Reset;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.ValidateToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _mediator.Send(command);
            var errorProperty = result.GetType().GetProperty("error");

            if (errorProperty != null)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateTokenCommand command)
        {

            var result = await _mediator.Send(command);
            var errorProperty = result.GetType().GetProperty("error");

            if (errorProperty != null)
                return BadRequest(result);
            return Ok(result);
        }
    }

}