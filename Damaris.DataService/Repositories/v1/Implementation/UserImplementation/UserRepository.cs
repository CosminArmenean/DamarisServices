using AutoMapper;
using Damaris.DataService.Data.v1;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
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
        private readonly IUserDataContext _userDataContext;
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
        public UserRepository(ILoggerFactory loggerFactory, IUnitOfWork unitOfWork, IMapper mapper) : base(loggerFactory, unitOfWork, mapper) 
        {
            //_userDataContext = userDataContext;
        }

                
        public Task ApplyUserPatchAsync<TEntity>(TEntity? entity, List<PatchDto>? patchDto) where TEntity : User
        {
            throw new NotImplementedException();
        }

        public Task CreateNewUserAsync(List<Damaris.Domain.v1.Dtos.UserDtos.RegisterUserDto>? user, bool twoUser = false)
        {
            throw new NotImplementedException();
        }

        public async Task DeactivateUserAsync(Guid userId)
        {
            var user = await _userDataContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                _userDataContext.Users.Remove(user);
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

        public async Task<User> GetUserByEmail(string email) 
        {
            return await _userDataContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        }

        public Task CreateNewUserAsync(List<Damaris.Domain.v1.Models.User.RegisterUser>? user, bool twoUser = false)
        {
            throw new NotImplementedException();
        }
    }
}
