using Damaris.Domain.v1.Models.User;

namespace Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces
{
    public interface IJwtTokenProvider
    {
        Task<string> CreateToken(JwtClaims claims);
    }
}
