using Damaris.Domain.v1.Dtos.Requests.User;
using Damaris.Domain.v1.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.Requests.Account
{
    public class AccountRegistrationRequestDto
    {
        [Required(ErrorMessage = "Accounts is required")]
        public List<UserRegistrationRequestDto> Accounts { get; set; }

        [Required(ErrorMessage = "RegisterTwoUser field is required.")]
        [Display(Name = "Register Two User")]
        public bool IsJointAccount { get; init; }

      
        public bool SharedAccount { get; init; }
    }
}
