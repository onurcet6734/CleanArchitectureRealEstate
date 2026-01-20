using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlatPartial;
using CleanArchitectureRealEstate.Domain.ValueObjects;
using MediatR;

public class UpdateFlatPartialCommandHandler
    : IRequestHandler<UpdateFlatPartialCommand, Result>
{
    private readonly IFlatRepository _flatRepository;
    private readonly ICurrentUserService _currentUser;

    public UpdateFlatPartialCommandHandler(IFlatRepository flatRepository , ICurrentUserService currentUser)
    {
        _flatRepository = flatRepository;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        UpdateFlatPartialCommand request,
        CancellationToken cancellationToken)
    {
        var flat = await _flatRepository.GetByIdAsync(request.Id, cancellationToken);

        if (flat is null)
            return Result.Failure("Flat not found");

        if (flat.UserId != _currentUser.UserId)
            return Result.Failure("You can not update another user's flat information!");

        if (request.Title is not null)
            flat.Title = request.Title;

        if (request.Description is not null)
            flat.Description = request.Description;

        if (request.Price.HasValue)
            flat.Price = request.Price.Value;

        if (request.Currency is not null)
            flat.Currency = request.Currency;

        if (request.City is not null)
            flat.City = request.City;

        if (request.District is not null)
            flat.District = request.District;

        if (request.AddressLine is not null)
            flat.AddressLine = request.AddressLine;

        if (request.Status is not null)
            flat.Status = FlatStatus.From(request.Status);

        if (request.Type is not null)
            flat.Type = FlatType.From(request.Type);

        flat.Updated = DateTime.UtcNow;

        await _flatRepository.UpdateAsync(flat, cancellationToken);

        return Result.Success();
    }
}
