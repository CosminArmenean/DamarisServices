using AutoMapper;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;

namespace Damaris.DataService.MappingProfiles.v1
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<RegisterUserDto, RegisterUser>()
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email));
        }
    }
}
