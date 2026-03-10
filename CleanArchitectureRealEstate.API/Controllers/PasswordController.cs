using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/password")]
    public class PasswordController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher _passwordHasher;

        public PasswordController(
            IUserRepository userRepository,
            IEmailService emailService,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, HttpContext.RequestAborted);
            
            // Security: Don't reveal if email exists
            if (user is null)
                return Ok(new { message = "Eğer e-posta adresiniz sistemde kayıtlı ise, şifre sıfırlama linki gönderilecektir." });

            // Generate reset token
            var resetToken = Guid.NewGuid().ToString("N");
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, HttpContext.RequestAborted);

            // Send email
            await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken, HttpContext.RequestAborted);

            return Ok(new { message = "Eğer e-posta adresiniz sistemde kayıtlı ise, şifre sıfırlama linki gönderilecektir." });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userRepository.GetByPasswordResetTokenAsync(request.Token, HttpContext.RequestAborted);

            if (user is null)
                return BadRequest(new { error = "Geçersiz veya süresi dolmuş token." });

            if (user.PasswordResetTokenExpires < DateTime.UtcNow)
                return BadRequest(new { error = "Token süresi dolmuş. Lütfen yeni bir şifre sıfırlama talebi oluşturun." });

            // Update password
            user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, HttpContext.RequestAborted);

            return Ok(new { message = "Şifreniz başarıyla güncellendi." });
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateTokenRequest request)
        {

            var user = await _userRepository.GetByPasswordResetTokenAsync(request.Token, HttpContext.RequestAborted);

            if (user is null || user.PasswordResetTokenExpires < DateTime.UtcNow)
                return BadRequest(new { valid = false, error = "Geçersiz veya süresi dolmuş token." });

            return Ok(new { valid = true });
        }
    }

}
