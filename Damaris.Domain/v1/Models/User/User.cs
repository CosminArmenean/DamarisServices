using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.User
{
    public class User : IdentityUser
    {
        public Guid UserId { get; init; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? MobilePhone { get; init; }

        public string ProfileImage { get; set; }
    }
}
