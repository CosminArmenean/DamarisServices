using Microsoft.AspNetCore.Identity;

namespace Damaris.Officer.Domain.v1.User
{
    public class ApplicationUser : IdentityUser
    {

        public bool IsJointAccount { get; set; }
    }
}
