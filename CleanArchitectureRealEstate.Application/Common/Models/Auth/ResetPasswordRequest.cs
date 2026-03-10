namespace CleanArchitectureRealEstate.Application.Common.Models.Auth;

public record ResetPasswordRequest
{
    public string Token { get; set; }

    public string NewPassword { get; set; }
}
