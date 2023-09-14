using AutoMapper;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;

namespace Damaris.DataService.MappingProfiles.v1.RequestToDomain
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName));
           
        }
                
    }
}
