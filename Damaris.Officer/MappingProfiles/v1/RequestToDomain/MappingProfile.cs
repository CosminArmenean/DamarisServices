using AutoMapper;
using Damaris.Domain.v1.Dtos.Requests.User;
using Microsoft.AspNetCore.Identity;

namespace Damaris.Officer.MappingProfiles.v1.RequestToDomain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationRequestDto, IdentityUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
                
        }
    }
}
