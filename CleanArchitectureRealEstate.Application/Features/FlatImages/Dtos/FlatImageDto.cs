using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings
{
    public class FlatImageDto
    {
        public int? Id { get; set; }
        public string Url { get; set; }
        public bool? IsCover { get; set; }

        public FlatDto Flat { get; set; }
    }


}
