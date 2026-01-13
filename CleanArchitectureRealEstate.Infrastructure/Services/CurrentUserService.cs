using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

public class CurrentUserService : ICurrentUserService
{
    public int UserId { get; }
    public bool IsAuthenticated { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            return;

        IsAuthenticated = true;

        var userIdClaim =
            user.FindFirst(JwtRegisteredClaimNames.Sub)
            ?? user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new Exception("UserId claim not found in JWT token");

        UserId = int.Parse(userIdClaim.Value);
    }
}
