using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Models.Account;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Damaris.DataService.Providers.v1.User
{
    public class IdentityServerServiceProvider : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityServerServiceProvider(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        async Task IProfileService.GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
                        
            var user = await _userManager.FindByIdAsync(sub);
            if (user != null)
            {
                var principal = await _claimsFactory.CreateAsync(user);

                var claims = principal.Claims.ToList();

                // Add custom claims as needed
                claims.Add(new Claim("custom_claim", "custom_value"));

                context.IssuedClaims = claims;
            }
        }

        async Task IProfileService.IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);

            context.IsActive = user != null;
        }
    }
}
