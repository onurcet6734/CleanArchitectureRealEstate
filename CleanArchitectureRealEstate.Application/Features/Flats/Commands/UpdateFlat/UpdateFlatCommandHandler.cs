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

            if (flat.UserId != _currentUser.UserId)
                return Result.Failure("Unauthorized");

            flat.Title = request.Title;
            flat.Description = request.Description;
            flat.Price = request.Price;
            flat.Currency = request.Currency;
            flat.City = request.City; ;
            flat.District = request.District;
            flat.AddressLine = request.AddressLine;
            flat.Type = FlatType.From(request.Type);
            flat.Status = FlatStatus.From(request.Status);
            flat.Updated = DateTime.UtcNow;

            await _flatRepository.UpdateAsync(flat, cancellationToken);

            return Result.Success();
        }
    }
}