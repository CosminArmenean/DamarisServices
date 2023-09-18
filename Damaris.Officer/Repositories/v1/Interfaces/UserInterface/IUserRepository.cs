using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;

namespace Damaris.Officer.Repositories.v1.Interfaces.UserInterface
{
    public interface IUserRepository
    {
        /// <summary>
        /// This method create a new user in db.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateNewUserAsync(Accounts? user, bool twoUser = false);


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
    
        
      
    }
}
