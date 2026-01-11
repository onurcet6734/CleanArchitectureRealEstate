using AutoMapper;
using CleanArchitectureRealEstate.Application.Features.Flats.Dtos;
using CleanArchitectureRealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Mappings
{
    public class FlatProfile : Profile
    {
        public FlatProfile()
        {
            CreateMap<Flat, FlatDto>()
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.Value))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value));
        }
    }
}
