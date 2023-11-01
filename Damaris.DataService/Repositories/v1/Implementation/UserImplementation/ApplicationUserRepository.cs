using AutoMapper;
using Damaris.DataService.Data.v1;
using Damaris.DataService.Providers.v1.User;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Models.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Damaris.DataService.Repositories.v1.Implementation.UserImplementation
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUserRepository : BaseRepository, IUserPhoneStore<ApplicationUser>
    {
      
         #region ==================== [Private Properties] =========================

         private readonly UserProvider _usersTable;

         #endregion ==================== [Private Properties] =========================


         #region ==================== [Constructor] =========================

         public ApplicationUserRepository(ILoggerFactory loggerFactory, DamarisDbContext officerDbContext) : base(loggerFactory, officerDbContext)
         {
            // _usersTable = new UserProvider(loggerFactory, connectionFactory);
         }

         #endregion ==================== [Constructor] =========================



         #region ================== [IUserPhoneStore<ApplicationUser> Implementation] ====================

         /// <summary>
         /// Find user by phone number
         /// </summary>
         /// <param name="phoneNumber"></param>
         /// <returns></returns>
         public async Task<IActionResult> FindByPhoneAsync(string? phoneNumber)
         {
             if (phoneNumber == null)
             {
                 _logger.LogWarning("Find user by phone number can't be done. Phone number is null");
                 return new BadRequestResult();
             }
             ApplicationUser? applicationUser = await _usersTable.FindByPhoneNumberAsync(phoneNumber);
             if (applicationUser == null)
             {
                 return new NotFoundResult();
             }
             else
             {
                 return new OkObjectResult(applicationUser);
             }

         }

         #endregion ================== [IUserPhoneStore<ApplicationUser> Implementation] ====================
        
    }
}
