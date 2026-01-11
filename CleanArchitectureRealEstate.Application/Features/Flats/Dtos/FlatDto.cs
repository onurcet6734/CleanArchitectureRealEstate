using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Dtos
{
    public class FlatDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public string Currency { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
    }
}
