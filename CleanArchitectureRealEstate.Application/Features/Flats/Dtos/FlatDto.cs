using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Dtos
{
    public class FlatDto
    {
        // Burası ayrıca Json Response ta sunulan property leri temsil ediyor.
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; } = default!;
        public string Currency { get; set; } = default!;
        public string? City { get; set; } = default!;
        public string? District { get; init; }
        public string? AddressLine { get; init; }
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

    }
}
