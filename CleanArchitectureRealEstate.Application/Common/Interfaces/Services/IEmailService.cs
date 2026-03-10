namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetToken, CancellationToken cancellationToken = default);
        Task SendEDevletVerificationEmailAsync(string email, string verificationUrl, CancellationToken cancellationToken = default);
    }
}
