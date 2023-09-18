using Damaris.Officer.Data.v1;

namespace Damaris.Officer.Repositories.v1
{
    public abstract class BaseRepository
    {
        protected ILogger _logger;
        protected readonly OfficerDbContext _officerDbContext;


        /// <summary>
        /// Base class for a repository implementation.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="connectionString">The database connection string.</param>
        public BaseRepository(ILoggerFactory loggerFactory, OfficerDbContext officerDbContext)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _officerDbContext = officerDbContext;

        }
    }
}
