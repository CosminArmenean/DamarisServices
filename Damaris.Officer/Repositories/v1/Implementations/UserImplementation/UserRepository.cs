using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Data.v1;
using Damaris.Officer.Repositories.v1.Interfaces.UserInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static IdentityServer4.Models.IdentityResources;

namespace Damaris.Officer.Repositories.v1.Implementations.UserImplementation
{
    /// <summary>
    /// identity user repository to store users in identity server
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly OfficerDbContext _officerDbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// User repository Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        public UserRepository(ILoggerFactory loggerFactory, OfficerDbContext officerDbContext) : base(loggerFactory, officerDbContext)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _officerDbContext = officerDbContext;
        }

        public async Task<IdentityUser> UpdateUserAsync(IdentityUser identityUser)
        {
            EntityEntry<IdentityUser> result = _officerDbContext.Users.Update(identityUser);
            if (result.Entity != null)
            {
                await _officerDbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<IdentityUser> CreateNewUserAsync(IdentityUser identityUser)
        {
            EntityEntry<IdentityUser> result = _officerDbContext.Users.Add(identityUser);
            if(result.Entity != null)
            {
                await _officerDbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<bool> DeactivateUserAsync(IdentityUser identityuser)
        {
            EntityEntry<IdentityUser> result = _officerDbContext.Users.Remove(identityuser);
            if (result.Entity != null)
            {
                await _officerDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
            
      
        public async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            var user = await _officerDbContext.Users.SingleOrDefaultAsync(u => u.Email == email) ?? null;
            if (user == null)
            {
                return user;
            }
            return null;
        }

        public async Task<IdentityUser> GetUserByPhoneAsync(string phone)
        {
            var user = await _officerDbContext.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phone) ?? null;
            if (user == null)
            {
                return user;
            }
            return null;
        }

        public async Task<IdentityUser> GetUserByUsernameAsync(string username)
        {
            var user = await _officerDbContext.Users.SingleOrDefaultAsync(u => u.UserName == username) ?? null;
            if (user == null)
            {

                return user;
            }
            return null;
        }
    }
}
