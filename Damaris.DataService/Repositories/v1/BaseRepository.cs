using AutoMapper;
using Damaris.DataService.Data.v1;
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
