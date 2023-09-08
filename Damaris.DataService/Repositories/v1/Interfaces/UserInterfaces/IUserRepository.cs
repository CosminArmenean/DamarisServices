using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// This method create a new user in db.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateNewUserAsync(List<Damaris.Domain.v1.Models.User.RegisterUser>? user, bool twoUser = false);


        /// <summary>
        /// This method patch a user property
        /// </summary>
        /// <param name="user"></param>
        /// <param name="patchDto"></param>
        /// <returns></returns>
        Task ApplyUserPatchAsync<TEntity>(TEntity? entity, List<PatchDto>? patchDto) where TEntity : User;

        /// <summary>
        /// This method is deactivating an usser account.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task DeactivateUserAsync(Guid userId);

        /// <summary>
        /// This method retrieve an active user from database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task GetUserAsync(Guid userId);

        Task<User> GetUserByEmail(string email);

        /// <summary>
        /// This method retrieve users active user from database and return as IEnumerable list.
        /// </summary>       
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsersAsync();
    }
}
