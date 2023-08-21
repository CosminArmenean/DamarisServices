using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
using Dapper;
using Microsoft.Extensions.Logging.Abstractions;
using MySql.Data.MySqlClient;
using System.Data;

namespace Damaris.DataService.Repositories.v1.BusinessRoutines
{
    public static class UserRoutines
    {
        #region =================== [Procedures] =========================

        private static readonly string PROCEDURE_GET_USERS = @"sp_getusers";

        #endregion =================== [Procedures] =========================


        private static ILogger _logger;
        private static IDatabaseConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializator of the User routines
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        public static void Init(ILoggerFactory loggerFactory, IDatabaseConnectionFactory connectionFactory)
        {
            _logger = loggerFactory != null ? loggerFactory.CreateLogger("User") : NullLogger.Instance;
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// This method chekd if model RegisterUserDto contains one users or contains two linked users
        /// </summary>
        /// <param name="userRegisterDto"></param>
        /// <returns></returns>
        public static async Task<List<RegisterUserDto?>?> ConvertToRegisterUser(this RegisterUser userRegisterDto, List<string>? guids = null)
        {
            try
            {
                List<int>? countrysIds = new();
                if (userRegisterDto != null)
                {
                    //getting country id for linked user
                    int? userCountryId = await GetCountryIdByCode(userRegisterDto.CountryCode);
                    countrysIds.Add(userCountryId.GetValueOrDefault());

                    //checking if are registering two users
                    if (userRegisterDto.RegisterTwoUser && userRegisterDto.LinkedUser != null)
                    {
                        //getting country id for linked user
                        int? secondUserCountryId = await GetCountryIdByCode(userRegisterDto.LinkedUser.CountryCode);
                        countrysIds.Add(secondUserCountryId.GetValueOrDefault());
                    }
                    //convert RegisterUserDto object to RegisterUser object
                    //List<RegisterUser?>? registerUsers = userRegisterDto.RegisterUserDtoAsRegisterUser(countrysIds, guids);
                    _logger.LogInformation($"Successfully cast to Registeruser object");
                    //return registerUsers;
                    return null;
                }
                else
                {
                    _logger.LogWarning($"Model RegisterUserDto is null");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failing checking if model contains two users");
                return null;
            }
        }


        /// <summary>
        /// Get country id by a given code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async static Task<int?> GetCountryIdByCode(string? code)
        {
            if (code == null) { return null; }
            _logger.LogInformation($"Retrieving country id for code: {code}.");
            await using MySqlConnection? mySqlConnection = await _connectionFactory.CreateConnectionAsync();
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
