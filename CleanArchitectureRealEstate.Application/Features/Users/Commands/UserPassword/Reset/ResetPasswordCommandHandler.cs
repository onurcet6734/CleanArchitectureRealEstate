using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.Reset
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand , object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<object> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByPasswordResetTokenAsync(request.Token, cancellationToken);

            if (user is null)
                return new { error = "Geçersiz veya süresi dolmuş token." };

            if (user.PasswordResetTokenExpires < DateTime.UtcNow)
                return new { error = "Token süresi dolmuş. Lütfen yeni bir şifre sıfırlama talebi oluşturun." };

            // Update password
            user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);
            return new { message = "Şifreniz başarıyla güncellendi." };
        }
    }
}
