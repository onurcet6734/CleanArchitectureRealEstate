using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.DeleteFlat
{
    public class DeleteFlatCommandHandler : IRequestHandler<DeleteFlatCommand, Result>
    {
        private readonly IFlatRepository _flatRepository;
        private readonly ICurrentUserService _currentUser;

        public DeleteFlatCommandHandler(
            IFlatRepository flatRepository,
            ICurrentUserService currentUser)
        {
            _flatRepository = flatRepository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(DeleteFlatCommand request, CancellationToken cancellationToken)
        {
            var flat = await _flatRepository.GetByIdAsync(request.FlatId, cancellationToken);
            if (flat is null)
                return Result.Failure("Flat not found");

            if (flat.User.Id != _currentUser.UserId)
                return Result.Failure("Unauthorized");

            flat.IsDeleted = true;
            flat.Updated = DateTime.UtcNow;

            return Result.Success();
        }
    }
}