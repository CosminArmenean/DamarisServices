using Damaris.DataService.Repositories.v1.BusinessRoutines;
using Damaris.DataService.Repositories.v1.Implementation.UserImplementation;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Damaris.DataService.Repositories.v1
{
    public class RepositoryCollection
    {
        #region ===================== [Private Properties] =====================

        private readonly IConfiguration _config;
        private readonly ILoggerFactory _logger;
        private readonly string _connectionString;
        private readonly IDatabaseConnectionFactory _connectionFactory;

        #endregion ===================== [Private Properties] =====================


        #region ===================== [Public Properties] =====================

        public IDatabaseConnectionFactory ConnectionFactory { get; private init; }
        public IUserRepository UserRepository { get; private init; }       
        public IUserStore<ApplicationUser> UserStore { get; private init; }



        #endregion ===================== [Public Properties] =====================


        #region ===================== [Constructor] =====================

        /// <summary>
        /// Initializes the database repositories.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="loggerFactory"></param>
        public RepositoryCollection(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _config = configuration;
            _logger = loggerFactory;
            _connectionString = "";

            // Initialize connection factory
            ConnectionFactory = null; ;
            _connectionFactory = ConnectionFactory;

            // Initialize the business routine classes            
         //   UserDataContext.Init(_logger, _connectionFactory);

            // Initialize the identity with dapper stores

            //UserStore = new UserStore(_logger, _connectionFactory);
            //RoleStore = new RoleStore(_logger, _connectionFactory);

            // Initialize the different repositories
            //ApplicationUserRepository = new ApplicationUserRepository(_logger, _connectionFactory);
            ///RefreshTokens = new RefreshTokensRepository(_logger, _connectionFactory);
            //UserRepository = new UserRepository(_logger, _connectionFactory);
            //CountryRepository = new CountryRepository(_logger, _connectionFactory);
        }

        #endregion ===================== [Constructor] =====================

    }
}
