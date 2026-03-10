using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/edevlet")]
    public class EDevletController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public EDevletController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("initiate")]
        public IActionResult InitiateVerification()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var loginUrl = "https://giris.turkiye.gov.tr/Giris/gir";
            var authControllerUrl = "https://giris.turkiye.gov.tr/OAuth2AuthorizationServer/AuthorizationController";

            var clientId = _configuration["EDevlet:ClientId"];
            var redirectUri = _configuration["EDevlet:RedirectUri"];

            var state = GenerateState(userId!);

            // 1) İç URL (continue içindeki kısım)
            var continueUrl =
                $"{authControllerUrl}?" +
                $"response_type=code&" +
                $"client_id={clientId}&" +
                $"state={state}&" +
                $"scope=Kimlik-Dogrula;Iletisim-Bilgileri&" +
                $"redirect_uri={Uri.EscapeDataString(redirectUri)}";

            // 2) Dış URL (Giris/gir)
            var finalUrl =
                $"{loginUrl}?" +
                $"oauthClientId={clientId}&" +
                $"continue={Uri.EscapeDataString(continueUrl)}";

            return Ok(new { authUrl = finalUrl, state });
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
                return BadRequest(new { error = "Geçersiz e-Devlet yanıtı" });

            var userId = ExtractUserIdFromState(state);
            if (userId == 0)
                return BadRequest(new { error = "Geçersiz state parametresi" });

            var user = await _userRepository.GetByIdAsync(userId, HttpContext.RequestAborted);
            if (user is null)
                return NotFound(new { error = "Kullanıcı bulunamadı" });

            user.IsEDevletVerified = true;
            user.EDevletVerifiedAt = DateTime.UtcNow;
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, HttpContext.RequestAborted);

            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
            return Redirect($"{frontendUrl}/edevlet-success");
        }

        [Authorize]
        [HttpGet("status")]
        public async Task<IActionResult> GetVerificationStatus()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetByIdAsync(userId, HttpContext.RequestAborted);

            if (user is null)
                return NotFound(new { error = "Kullanıcı bulunamadı" });

            return Ok(new
            {
                isVerified = user.IsEDevletVerified,
                verifiedAt = user.EDevletVerifiedAt
            });
        }

        [Authorize]
        [HttpPost("simulate-verification")]
        public async Task<IActionResult> SimulateVerification()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetByIdAsync(userId, HttpContext.RequestAborted);

            if (user is null)
                return NotFound(new { error = "Kullanıcı bulunamadı" });

            user.IsEDevletVerified = true;
            user.EDevletVerifiedAt = DateTime.UtcNow;
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, HttpContext.RequestAborted);

            return Ok(new { message = "e-Devlet doğrulaması simüle edildi", isVerified = true });
        }

        private string GenerateState(string userId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var random = Guid.NewGuid().ToString("N")[..8];
            return $"{userId}_{timestamp}_{random}";
        }

        private int ExtractUserIdFromState(string state)
        {
            var parts = state.Split('_');
            if (parts.Length >= 1 && int.TryParse(parts[0], out var userId))
                return userId;
            return 0;
        }
    }
}
