using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.CreateFlat
{
    public class CreateFlatCommand : IRequest<FlatDto>
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

        public decimal Price { get; set; }
        public string Currency { get; set; } = default!;

        public string City { get; set; } = default!;
        public string District { get; set; } = default!;
        public string AddressLine { get; set; } = default!;

        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;

        public List<string>? ImageUrls { get; set; }

    }
}
