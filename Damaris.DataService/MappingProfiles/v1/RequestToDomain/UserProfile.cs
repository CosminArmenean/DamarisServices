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
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(src => src.EmailConfirmation))
                .ForMember(dest => dest.Country.Code,
                    opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.MobilePhone,
                    opt => opt.MapFrom(src => src.MobilePhone))
                .ForMember(dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender));
           
        }
                
    }
}
