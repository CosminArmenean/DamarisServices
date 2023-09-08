using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;

namespace Damaris.DataService.Repositories.v1.Interfaces.Generic
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task<bool> CompleteAsync();
        void BeginTransaction();
        void Commit();
        void Rollback();

    }
}
