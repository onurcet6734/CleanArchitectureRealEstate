namespace CleanArchitectureRealEstate.Application.Common.Models.Auth;

public record ValidateTokenRequest
{
    public string Token { get; set; }
}
