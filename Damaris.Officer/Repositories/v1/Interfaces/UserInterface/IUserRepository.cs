using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Dtos.Requests.User;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Damaris.Officer.Repositories.v1.Interfaces.UserInterface
{
    public interface IUserRepository
    {
        /// <summary>
        /// This method create a new user in db.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityUser> CreateNewUserAsync(IdentityUser userIdentity);


        /// <summary>
        /// This method patch a user property
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        Task<IdentityUser> UpdateUserAsync(IdentityUser identityUser);

        /// <summary>
        /// This method is deactivating an user account.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeactivateUserAsync(IdentityUser identityUser);

        /// <summary>
        /// This method is getting user by email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IdentityUser> GetUserByEmailAsync(string email);

        /// <summary>
        /// This method is getting user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<IdentityUser> GetUserByUsernameAsync(string username);

        /// <summary>
        /// This methid is getting user by phone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<IdentityUser> GetUserByPhoneAsync(string phone);

        
    
        
      
    }
}
