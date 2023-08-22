using Damaris.DataService.Repositories.v1.Interfaces.Contracts;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
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
        public UserRepository(ILoggerFactory loggerFactory, IDatabaseConnectionFactory connectionFactory) : base(loggerFactory, connectionFactory) { }

        

        public Task ApplyUserPatchAsync<TEntity>(TEntity? entity, List<PatchDto>? patchDto) where TEntity : User
        {
            throw new NotImplementedException();
        }

        public Task CreateNewUserAsync(List<RegisterUserDto>? user, bool twoUser = false)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task GetUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        
    }
}
