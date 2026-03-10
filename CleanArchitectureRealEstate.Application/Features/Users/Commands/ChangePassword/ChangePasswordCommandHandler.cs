using AutoMapper;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler
        : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUser;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(_currentUser.UserId, cancellationToken);

            if (user is null)
                throw new Exception("Kullanıcı bulunamadı");

            if (!_passwordHasher.Verify(request.OldPassword, user.PasswordHash))
                throw new Exception("Mevcut şifre yanlış");

            if (request.OldPassword == request.NewPassword)
                throw new Exception("Yeni şifre eski şifre ile aynı olamaz");

            user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return Result.Success();
        }
    }
}
