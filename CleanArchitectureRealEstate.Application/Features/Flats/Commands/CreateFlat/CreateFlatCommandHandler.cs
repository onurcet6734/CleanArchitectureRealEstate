using AutoMapper;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Domain.Exceptions;
using CleanArchitectureRealEstate.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.CreateFlat
{
    public class CreateFlatCommandHandler
     : IRequestHandler<CreateFlatCommand, FlatDto>
    {
        private readonly IFlatRepository _flatRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateFlatCommandHandler(
            IFlatRepository flatRepository,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _flatRepository = flatRepository;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<FlatDto> Handle(CreateFlatCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                throw new NotFoundException(nameof(User), userId);

            var flat = new Flat
            {
                UserId = userId,
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Currency = request.Currency,
                City = request.City,
                District = request.District,
                AddressLine = request.AddressLine,
                Type = FlatType.From(request.Type),
                Status = FlatStatus.From(request.Status),
                Created = DateTime.UtcNow
            };

            await _flatRepository.AddAsync(flat, cancellationToken);

            return _mapper.Map<FlatDto>(flat);
        }
    }
}
