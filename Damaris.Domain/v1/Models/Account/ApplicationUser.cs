using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
        public Guid UserId { get; set; }

        public bool IsActive { get; set; }

        public int UserType { get; set; }

        public List<Claim>? Claims { get; set; }
        public List<UserRole>? Roles { get; set; }
        public List<UserLoginInfo>? Logins { get; set; }
        public List<UserToken>? Tokens { get; set; }

        public ApplicationUser? LinkedUser { get; set; }
    }
}
