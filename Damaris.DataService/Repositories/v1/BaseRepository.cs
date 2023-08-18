using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.DataService.Repositories.v1
{
    public abstract class BaseRepository
    {
        protected ILogger _logger;
        protected IDatabaseConnectionFactory _connectionFactory;

        /// <summary>
        /// Base class for a repository implementation.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="connectionString">The database connection string.</param>
        public BaseRepository(ILoggerFactory loggerFactory, IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _logger = loggerFactory.CreateLogger(GetType());
        }
    }
}
