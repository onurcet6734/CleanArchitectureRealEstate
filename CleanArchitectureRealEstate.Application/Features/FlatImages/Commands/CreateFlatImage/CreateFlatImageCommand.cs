using CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings;
using MediatR;

public record CreateFlatImageCommand(
    int FlatId,
    string Url,
    bool IsCover
) : IRequest<FlatImageDto>;
