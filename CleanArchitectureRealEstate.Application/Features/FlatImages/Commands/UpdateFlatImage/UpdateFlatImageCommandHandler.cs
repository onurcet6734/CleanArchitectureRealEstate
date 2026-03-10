using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using CleanArchitectureRealEstate.Application.Common.Models;
using CleanArchitectureRealEstate.Application.Features.FlatImagess.Commands.UpdateFlatImage;
using MediatR;


namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Commands.Update
{
    public class UpdateFlatImageCommandHandler
    : IRequestHandler<UpdateFlatImageCommand, Result>
    {
        private readonly IFlatImageRepository _flatImageRepository;
        private readonly ICurrentUserService _currentUser;


        public UpdateFlatImageCommandHandler(
        IFlatImageRepository flatImageRepository,
        ICurrentUserService currentUser)
        {
            _flatImageRepository = flatImageRepository;
            _currentUser = currentUser;
        }


        public async Task<Result> Handle(
        UpdateFlatImageCommand request,
        CancellationToken cancellationToken)
        {
            var image = await _flatImageRepository
            .GetByIdWithFlatAsync(request.Id, cancellationToken);


            if (image is null)
                return Result.Failure("Flat image not found");


            if (image.Flat.UserId != _currentUser.UserId)
                return Result.Failure("You cannot update another user's flat image");


            if (request.Url is not null)
                image.Url = request.Url;


            if (request.IsCover.HasValue)
                image.IsCover = request.IsCover.Value;


            image.Updated = DateTime.UtcNow;


            await _flatImageRepository.UpdateAsync(image, cancellationToken);


            return Result.Success();
        }
    }
}