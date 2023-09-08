using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces
{
    public interface IUserDataContext
    {
        DbSet<User> Users { get; }
    }
}
