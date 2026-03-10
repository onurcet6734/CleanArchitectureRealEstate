using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateProfileCommandHandler(IUserRepository userRepository, ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_currentUser.UserId, cancellationToken);
            if (user is null)
            {
                return Result.Failure("Kullanıcı bulunamadı");
            }

            if (request.Email != user.Email)
            {
                var emailExists = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
                if (emailExists)
                {
                    return Result.Failure("Bu e-posta adresi zaten kullanılıyor");
                }

                user.Email = request.Email;
            }

            // Diğer alanları güncelle
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return Result.Success();


        }

    }
}
