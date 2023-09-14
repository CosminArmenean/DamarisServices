using Damaris.Domain.v1.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.Requests.Account
{
    public class AccountRegistrationRequestDto
    {
        public string RequestType { get; init; }

        [Required(ErrorMessage = "RegisterTwoUser field is required.")]
        [Display(Name = "Register Two User")]
        public bool IsJointAccount { get; init; }        
        public RegisterUserDto User { get; init; }
        public RegisterUserDto JointUser { get; init; }


    }
}
