using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.UploadFlatImage;

public record UploadImageCommand(List<IFormFile>? Files) : IRequest<List<Dictionary<string, object>>>;
