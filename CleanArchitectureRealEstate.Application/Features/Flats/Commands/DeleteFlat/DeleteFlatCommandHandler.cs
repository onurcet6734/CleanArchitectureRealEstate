using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;

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
            var flat = await _flatRepository.GetByIdAsync(request.Id, cancellationToken);

            if (flat is null)
                return Result.Failure("Flat not found");

            await _flatRepository.DeleteAsync(flat, cancellationToken);

            return Result.Success();
        }
    }
}