using AutoMapper;
using Damaris.DataService.Data.v1;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.DataService.Repositories.v1.Implementation.UserImplementation
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly OfficerDbContext _officerDbContext;
        private readonly ILogger _logger;
        //procedures names
        #region ================ [Procedures] =========================

        private static readonly string PROCEDURE_INSERT_USER = @"sp_insertuser";
        private static readonly string PROCEDURE_GET_USER = @"sp_getuser";
        private static readonly string PROCEDURE_DEACTIVATE_USER = @"sp_deactivateuser";
        private static readonly string PROCEDURE_PATCH_USER = @"sp_patchuser";

        #endregion  ================ [Procedures] =========================
         
        /// <summary>
        /// User repository Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        public UserRepository(ILoggerFactory loggerFactory, OfficerDbContext officerDbContext) : base(loggerFactory, officerDbContext) 
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _officerDbContext = officerDbContext;
        }

                
        public Task ApplyUserPatchAsync<TEntity>(TEntity? entity, List<PatchDto>? patchDto) where TEntity : User
        {
            throw new NotImplementedException();
        }

        public Task CreateNewUserAsync(List<Damaris.Domain.v1.Dtos.UserDtos.RegisterUserDto>? user, bool twoUser = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> RegisterUserAsync(object registrationDto)
        {
            if (registrationDto is RegisterUserDto singleUserDto)
            {
                // Single user registration
                var user = new User
                {
                    FirstName = singleUserDto.FirstName,
                    Email = singleUserDto.Email,
                    // Set other user properties
                };

                var result = await _officerDbContext.Users.AddAsync(user);
                return result;
            }
            else if (registrationDto is JointAccountRegistrationDto jointAccountDto)
            {
                // Joint account registration
                var primaryUser = new User
                {
                    UserName = jointAccountDto.PrimaryUsername,
                    Email = jointAccountDto.PrimaryEmail,
                    // Set other user properties
                };

                var result = await _userManager.CreateAsync(primaryUser, jointAccountDto.PrimaryPassword);

                if (result.Succeeded)
                {
                    // Create secondary users and associate them with the joint account
                    foreach (var secondaryUserDto in jointAccountDto.SecondaryUsers)
                    {
                        var secondaryUser = new User
                        {
                            UserName = secondaryUserDto.Username,
                            Email = secondaryUserDto.Email,
                            // Set other user properties
                        };

                        var secondaryResult = await _userManager.CreateAsync(secondaryUser);

                        if (secondaryResult.Succeeded)
                        {
                            // Associate secondary user with joint account
                            // You may need to implement logic to create a joint account and associate users here
                        }
                        else
                        {
                            // Handle secondary user registration failure
                            // You may want to roll back primary user registration here
                        }
                    }
                }

                return result;
            }

            // Handle unsupported registration type
            return IdentityResult.Failed(new IdentityError { Description = "Invalid registration type." });
        }
        public async Task DeactivateUserAsync(Guid userId)
        {
            var user = await _officerDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                _officerDbContext.Users.Remove(user);
               // await _userDataContext.SaveChangesAsync();
            }            
        }

        public Task GetUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                //return await DbSet.Where
                return null;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email) 
        {
            return await _officerDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        }

        public Task CreateNewUserAsync(List<Damaris.Domain.v1.Models.User.RegisterUser>? user, bool twoUser = false)
        {
            throw new NotImplementedException();
        }
    }
}
