using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models.Auth;
using CleanArchitectureRealEstate.Application.Features.Users.Commands.ChangePassword;
using CleanArchitectureRealEstate.Application.Features.Users.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _mediator.Send(new GetProfileByIdQuery(userId));

            if (result is null)
                return NotFound(new { error = "Kullanıcı bulunamadı" });

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand request)
        {
            var result = await _mediator.Send(request);

            if (result.Succeeded)
                return Ok(new { message = "Profil başarıyla güncellendi" });

            return BadRequest(new { error = result.Error ?? "Profil güncellenemedi" });
        }


        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
        {
            await _mediator.Send(request);
            return Ok(new { message = "Şifre başarıyla değiştirildi" });
        }

    }

}
