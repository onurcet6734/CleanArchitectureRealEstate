using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UserPassword.ValidateToken
{
    public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand , object>
    {
        private readonly IUserRepository _userRepository;

        public ValidateTokenCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<object> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByPasswordResetTokenAsync(request.Token, cancellationToken);

            if (user is null || user.PasswordResetTokenExpires < DateTime.UtcNow)
            {
                return new { valid = false, error = "Geçersiz veya süresi dolmuş token." };
            }
            return new { valid = true };
        }
    }
}
