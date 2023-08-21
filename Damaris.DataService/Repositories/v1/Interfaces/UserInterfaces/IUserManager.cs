using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;

namespace Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces
{
    public interface IUserManager
    {
        Task<ApplicationUser> FindByUsernameAsync(string username);
    }
}
