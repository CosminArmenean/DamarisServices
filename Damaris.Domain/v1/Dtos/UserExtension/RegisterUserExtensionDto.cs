using Damaris.Domain.v1.Dtos.UserDtos;
using Damaris.Domain.v1.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.UserExtension
{
    public static class RegisterUserExtensionDto
    {
        private static readonly int IS_ACTIVE = 1; //Value for an active user

        /// <summary>
        /// This method is encapsulating data from RegisterUserDto object and returning a RegisterUser object
        /// </summary>
        /// <param name="registerUser"></param>
        /// <param name="countryId"></param>
        /// <param name="linkedUserCountryId"></param>
        /// <returns></returns>
        public static List<RegisterUserDto?>? RegisterUserDtoAsRegisterUser(this RegisterUser registerUser, List<int>? countrysIds = null, List<string>? guids = null)
        {
            if (registerUser == null) //checking if is null in this case we will return null 
            {
                return null;
            }
            List<RegisterUserDto?> registerUserList = new();
            Guid guid = guids != null ? new Guid(guids[0]) : Guid.NewGuid();
            Guid guidLinkedUser = countrysIds.Count > 1 ? new Guid(guids[1]) : Guid.NewGuid();
            RegisterUserDto? user = new();
            RegisterUserDto? linkedUser = new();
            //casting objects to RegisterUser object
            user = new RegisterUserDto()
            {
                UserId = guid,
                FirstName = registerUser?.FirstName,
                LastName = registerUser?.LastName,
                PasswordHash = registerUser?.Password,
                Email = registerUser?.Email,
                CountryId = countrysIds != null ? countrysIds[0] : 0,
                MobilePhone = registerUser?.MobilePhone,
                BirthDate = registerUser.BirthDate,
                Gender = registerUser.Gender != null ? char.Parse(registerUser.Gender) : default,
                RegisteredAt = registerUser.RegisteredAt,
                IsActive = IS_ACTIVE,
                LastLogin = registerUser.RegisteredAt,
                LinkedWithAccount = registerUser.RegisterTwoUser == true ? guidLinkedUser : null
            };
            if (registerUser.RegisterTwoUser && registerUser.LinkedUser != null)
            {
                linkedUser = new RegisterUserDto()
                {
                    UserId = guidLinkedUser,
                    FirstName = registerUser.LinkedUser.FirstName,
                    LastName = registerUser.LinkedUser.LastName,
                    PasswordHash = registerUser.LinkedUser.Password,
                    Email = registerUser.LinkedUser.Email,
                    CountryId = countrysIds != null ? countrysIds[1] : 0,
                    MobilePhone = registerUser.LinkedUser.MobilePhone,
                    BirthDate = registerUser.LinkedUser.BirthDate,
                    Gender = registerUser.LinkedUser.Gender != null ? char.Parse(registerUser.LinkedUser.Gender) : default,
                    RegisteredAt = registerUser.LinkedUser.RegisteredAt,
                    IsActive = IS_ACTIVE,
                    LastLogin = registerUser.LinkedUser.RegisteredAt,
                    LinkedWithAccount = guid
                };
                //adding both user to list and returning the list
                registerUserList.Add(user);
                registerUserList.Add(linkedUser);
                return registerUserList;
            }
            else
            {
                linkedUser = null;
                //adding user to list and returning the list
                registerUserList.Add(user);
            }
            return registerUserList;
        }
    }
}