using Damaris.Officer.Repositories.v1.Interfaces.UserInterface;

namespace Damaris.Officer.Repositories.v1.Interfaces.Generic
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
