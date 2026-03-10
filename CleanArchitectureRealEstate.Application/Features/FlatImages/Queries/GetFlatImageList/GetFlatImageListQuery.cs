using MediatR;
using CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings;

namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetList
{
    public class GetFlatImageListQuery : IRequest<List<FlatImageDto>>
    {
        public int? UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public bool? IsCover { get; set; }
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
