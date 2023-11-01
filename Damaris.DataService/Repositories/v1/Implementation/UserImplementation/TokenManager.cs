using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Damaris.DataService.Repositories.v1.Implementation.UserImplementation
{
    public class TokenManager
    {
        private readonly IUserManager _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public TokenManager(IUserManager userManager, IJwtTokenProvider jwtTokenProvider)
        {
            _userManager = userManager;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<string> CreateAsync(User user)
        {
            var claims = new JwtClaims() { Subject = "",Issuer = "Damaris.DataService", Audience = "DamarisServices", ExpiresIn = 1693384435 };
                

            var token = await _jwtTokenProvider.CreateToken(claims);

            return token;            
        }
    }
}
