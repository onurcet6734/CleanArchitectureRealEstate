using AutoMapper;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Application.Features.Users.Dtos;

namespace CleanArchitectureRealEstate.Application.Features.Users.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
