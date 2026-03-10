using CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings;
using MediatR;
using Microsoft.AspNetCore.Http;

public record CreateFlatImageCommand(
    int FlatId,
    IFormFile Image,
    bool IsCover
) : IRequest<FlatImageDto>;