using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using MediatR;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList
{
    public class GetFlatListQuery : IRequest<List<FlatDto>>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? City { get; set; }
        public string? District { get; set; }

        public string? Title { get; set; }
        public string? AddressLine { get; init; }
        public string? Description { get; init; }
        public string? Status { get; set; }
        public string? Type { get; set; }

    }
}