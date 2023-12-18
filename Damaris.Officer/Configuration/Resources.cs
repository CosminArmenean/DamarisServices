using IdentityModel;
using IdentityServer4.Models;

namespace Damaris.Officer.Configuration
{
    public class Resources
    {
        // identity resources represent identity data about a user that can be requested via the scope parameter (OpenID Connect)
        public static readonly IEnumerable<IdentityResource> IdentityResources =
            new[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),               
                // custom identity resource with some consolidated claims
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }
                //new IdentityResource("custom.profile", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, "location" })
            };

        // API scopes represent values that describe scope of access and can be requested by the scope parameter (OAuth)
        public static readonly IEnumerable<ApiScope> ApiScopes =
            new[]
            {
                // local API scope
                new ApiScope(IdentityServer4.IdentityServerConstants.LocalApi.ScopeName),

                // resource specific scopes
                new ApiScope("innkt.read"),
                new ApiScope("innkt.write"), 
                
                // a scope without resource association
                new ApiScope("scope3"),
                
                // a scope shared by multiple resources
                new ApiScope("shared.scope"),

                // a parameterized scope
                new ApiScope("transaction", "Transaction")
                {
                    Description = "Some Transaction"
                }
            };

        // API resources are more formal representation of a resource with processing rules and their scopes (if any)
        public static readonly IEnumerable<ApiResource> ApiResources =
            new[]
            {
                new ApiResource("innkt", "innkt")
                {
                    ApiSecrets = { new Secret("ScopeSecret".Sha256()) },

                    Scopes = { "innkt.read", "innkt.write" },

                    UserClaims = new List<string> { "role" }
                },
                
                //// expanded version if more control is needed
                //new ApiResource("resource2", "Resource 2")
                //{
                //    ApiSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    // additional claims to put into access token
                //    UserClaims =
                //    {
                //        JwtClaimTypes.Name,
                //        JwtClaimTypes.Email
                //    },

                //    Scopes = { "resource2.scope1", "shared.scope" }
                //}
            };


        public static IEnumerable<Client> Clients =>
           new[]
           {
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("ClientSecret1".Sha256())},
                    AllowedScopes = { "innkt.read", "innkt.write"}
                },
                //interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("ClientSecret1".Sha256())},                   
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:4200/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:4200/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:4200/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "innkt.read"},
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                }
           };
   }
}
