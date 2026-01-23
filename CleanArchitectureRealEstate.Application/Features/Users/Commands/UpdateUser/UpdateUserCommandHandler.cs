using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using CleanArchitectureRealEstate.Domain.Exceptions;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user is null)
            {
                return Result.Failure("User not found");
            }

            // Authorization check
            if (user.Id != _currentUserService.UserId)
            {
                throw new UnauthorizedDomainException("You cannot update another user's account information");
            }

            // Only update fields that are provided (partial update)
            if (request.FirstName != null)
            {
                user.FirstName = request.FirstName;
            }

            if (request.LastName != null)
            {
                user.LastName = request.LastName;
            }

            user.Updated = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return Result.Success();
        }
    }
}