using AutoMapper;
using Damaris.DataService.Data.v1;
using Damaris.DataService.Repositories.v1;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.Domain.v1.Models.Account;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace Damaris.DataService.Providers.v1.User
{
    internal class UserProvider : BaseRepository
    {
        #region ==================== [Private Properties] ====================

        private readonly string SP_GET_APPLICATION_USER_BY_PHONE = "sp_getapplicationuserbyphone";

        #endregion ==================== [Private Properties] ====================

        #region ==================== [Constructor] ====================

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionFactory"></param>
        public UserProvider(ILoggerFactory loggerFactory, OfficerDbContext officerDbContext) : base(loggerFactory, officerDbContext)
        {

        }

        #endregion ==================== [Constructor] ====================



        #region ====================== [Public Methods] ===================

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var command = "SELECT * FROM AspNetUsers;";
            await using MySqlConnection? mySqlConnection = null;//await _connectionFactory.CreateConnectionAsync();

            return await mySqlConnection.QueryAsync<ApplicationUser>(command);
        }


        /// <summary>
        /// Find user by phone and return a user as indentity
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            await using MySqlConnection mySqlConnection = null;// await _connectionFactory.CreateConnectionAsync();
            return await mySqlConnection.QuerySingleOrDefaultAsync<ApplicationUser>(SP_GET_APPLICATION_USER_BY_PHONE, new
            {
                PhoneNumberVal = phoneNumber
            }, commandType: CommandType.StoredProcedure);
        }
        #endregion ====================== [Public Methods] ===================
    }
}
