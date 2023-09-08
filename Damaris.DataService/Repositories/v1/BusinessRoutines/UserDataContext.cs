using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MySql.Data.MySqlClient;
using System.Data;

namespace Damaris.DataService.Repositories.v1.BusinessRoutines
{
    public class UserDataContext : DbContext, IUserDataContext
    {
        #region =================== [Procedures] =========================

        private readonly string PROCEDURE_GET_USERS = @"sp_getusers";

        #endregion =================== [Procedures] =========================


        private ILogger _logger;       
        public DbSet<User> Users { get; set; }

        public UserDataContext(DbContextOptions<UserDataContext> options) : base (options)
        {
                
        }



        ///// <summary>
        ///// This method chekd if model RegisterUserDto contains one users or contains two linked users
        ///// </summary>
        ///// <param name="userRegisterDto"></param>
        ///// <returns></returns>
        //public static async Task<List<RegisterUser?>?> ConvertToRegisterUser(this RegisterUserDto userRegisterDto, List<string>? guids = null)
        //{
        //    try
        //    {
        //        List<int>? countrysIds = new();
        //        if (userRegisterDto != null)
        //        {
        //            //getting country id for linked user
        //            int? userCountryId = await GetCountryIdByCode(userRegisterDto.CountryCode);
        //            countrysIds.Add(userCountryId.GetValueOrDefault());

        //            //checking if are registering two users
        //            if (userRegisterDto.RegisterTwoUser && userRegisterDto.LinkedUser != null)
        //            {
        //                //getting country id for linked user
        //                int? secondUserCountryId = await GetCountryIdByCode(userRegisterDto.LinkedUser.CountryCode);
        //                countrysIds.Add(secondUserCountryId.GetValueOrDefault());
        //            }
        //            //convert RegisterUserDto object to RegisterUser object
        //            List<RegisterUser?>? registerUsers = userRegisterDto.RegisterUserDtoAsRegisterUser(countrysIds, guids);
        //            _logger.LogInformation($"Successfully cast to Registeruser object");
        //            return registerUsers;
        //        }
        //        else
        //        {
        //            _logger.LogWarning($"Model RegisterUserDto is null");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Failing checking if model contains two users");
        //        return null;
        //    }
        //}


        /// <summary>
        /// Get country id by a given code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<int?> GetCountryIdByCode(string? code)
        {
            if (code == null) { return null; }
            _logger.LogInformation($"Retrieving country id for code: {code}.");
            await using MySqlConnection? mySqlConnection = null;// await _connectionFactory.CreateConnectionAsync();
            try
            {
                string? procedure = "sp_getcountryidbycode";
                object? values = new { CountryIdVal = code.ToUpper() };
                IEnumerable<int> enumerable = await mySqlConnection.QueryAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                if (enumerable.Any())
                {
                    int? countryId = enumerable.First();
                    _logger.LogInformation($"Successfully retrieving country id {countryId} for code {code}");

                    return countryId;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve country id for code {code}.");
                return null;
            }
        }

    }
}
