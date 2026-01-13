using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using CleanArchitectureRealEstate.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlat
{
    public class UpdateFlatCommandHandler : IRequestHandler<UpdateFlatCommand, Result>
    {
        private readonly IFlatRepository _flatRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateFlatCommandHandler(
            IFlatRepository flatRepository,
            ICurrentUserService currentUser)
        {
            _flatRepository = flatRepository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateFlatCommand request, CancellationToken cancellationToken)
        {
            var flat = await _flatRepository.GetByIdAsync(request.Id, cancellationToken);
            if (flat is null)
                return Result.Failure("Flat not found");

            if (flat.User.Id != _currentUser.UserId)
                return Result.Failure("Unauthorized");

            flat.Price = request.Price;
            flat.Status = FlatStatus.From(request.Status);
            flat.Updated = DateTime.UtcNow;

            return Result.Success();
        }
    }
}