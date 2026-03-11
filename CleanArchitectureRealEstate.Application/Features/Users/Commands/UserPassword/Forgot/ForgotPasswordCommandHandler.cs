using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Forgot
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand , object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<object> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            // Security: Don't reveal if email exists
            if (user is null)
                return new { message = "Eğer e-posta adresiniz sistemde kayıtlı ise, şifre sıfırlama linki gönderilecektir." };

            // Generate reset token
            var resetToken = Guid.NewGuid().ToString("N");
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            // Send email
            await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken , cancellationToken);

            return new { message = "Eğer e-posta adresiniz sistemde kayıtlı ise, şifre sıfırlama linki gönderilecektir." };

        }
    }
}
