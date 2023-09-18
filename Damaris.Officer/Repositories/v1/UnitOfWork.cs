using Damaris.Officer.Data.v1;
using Damaris.Officer.Repositories.v1.Implementations.UserImplementation;
using Damaris.Officer.Repositories.v1.Interfaces.Generic;
using Damaris.Officer.Repositories.v1.Interfaces.UserInterface;

namespace Damaris.Officer.Repositories.v1
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly OfficerDbContext _officerDbContext;
        private readonly ILoggerFactory _loggerFactory;
        private bool _transactionStarted;
        public IUserRepository Users { get; }

        public UnitOfWork(ILoggerFactory loggerFactory, OfficerDbContext officerDbContext)
        {
            _officerDbContext = officerDbContext;
            _loggerFactory = loggerFactory;

            Users = new UserRepository(_loggerFactory, _officerDbContext);
        }

        public async Task<bool> CompleteAsync()
        {
            var result = await _officerDbContext.SaveChangesAsync();
            return result > 0;
        }



        public void BeginTransaction()
        {
            if (_transactionStarted)
                throw new InvalidOperationException("Transaction has already started.");

            _officerDbContext.Database.BeginTransaction();
            _transactionStarted = true;
        }

        public void Commit()
        {
            if (!_transactionStarted)
                throw new InvalidOperationException("Transaction has not been started.");

            _officerDbContext.SaveChanges();
            _officerDbContext.Database.CommitTransaction();
            _transactionStarted = false;
        }

        public void Rollback()
        {
            if (!_transactionStarted)
                throw new InvalidOperationException("Transaction has not been started.");

            _officerDbContext.Database.RollbackTransaction();
            _transactionStarted = false;
        }

        public void Dispose()
        {
            if (_transactionStarted)
            {
                _officerDbContext.Database.RollbackTransaction();
                _transactionStarted = false;
            }
            _officerDbContext.Dispose();
        }
    }
}
