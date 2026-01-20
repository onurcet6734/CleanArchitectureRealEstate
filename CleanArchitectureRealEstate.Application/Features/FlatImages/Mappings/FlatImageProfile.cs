using AutoMapper;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.FlatImages.Mappings
{
    public class FlatImageProfile : Profile
    {
        public FlatImageProfile()
        {
            CreateMap<CleanArchitectureRealEstate.Domain.Entities.FlatImage, FlatImageDto>();

            CreateMap<Flat, FlatDto>();
        }
    }
}
