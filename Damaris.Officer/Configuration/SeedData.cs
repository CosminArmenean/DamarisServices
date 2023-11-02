using Damaris.Officer.Data.v1;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Damaris.Officer.Configuration
{
    public class SeedData
    {
        /// <summary>
        /// Should run once when app first run to seed data to db 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<OfficerDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<OfficerDbContext>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                    db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
                });

            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                    db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));

                });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<OfficerDbContext>();
            ctx.Database.Migrate();
            EnsureUsers(scope);

        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var damaris = userMgr.FindByNameAsync("damaris").Result;
            if(damaris == null)
            {
                damaris = new IdentityUser
                {
                    UserName = "damaris",
                    Email = "damaris@army.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(damaris, "Parola@123").Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(
                        damaris,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Damaris Armenean"),
                            new Claim(JwtClaimTypes.GivenName, "Damaris"),
                            new Claim(JwtClaimTypes.FamilyName, "Armenean"),
                            new Claim(JwtClaimTypes.WebSite, "http://ink-tink.com"),
                            new Claim("location", "somewhere")
                        }
                    ).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }


        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Resources.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Resources.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var scope in Resources.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }
                context.SaveChanges();
            }


            if (!context.ApiResources.Any())
            {
                foreach (var resource in Resources.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
