using AutoMapper;
using Damaris.Domain.v1.Dtos.Responses.Account;
using Damaris.Domain.v1.Models.User;

namespace Damaris.DataService.MappingProfiles.v1.DomainToResponse
{
    public class ResponseUserProfile : Profile
    {
        public ResponseUserProfile() 
        {
            CreateMap<User, AccountRegistrationResponse>()
                .ForMember(dest => dest.AccountId,
                    opt => opt.MapFrom(src => src.FirstName));
        }
    }
}
