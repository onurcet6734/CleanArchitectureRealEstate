using AutoMapper;
using MediatR;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Domain.Exceptions;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.Users.Dtos;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;

namespace CleanArchitectureRealEstate.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            // Check if email already exists
            if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            {
                throw new ValidationDomainException("Email is already registered");
            }

            // Check if username already exists
            if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
            {
                throw new ValidationDomainException("Username is already taken");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                Created = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user, cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
    }
}