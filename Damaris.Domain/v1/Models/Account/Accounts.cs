using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damaris.Domain.v1.Models.User;

namespace Damaris.Domain.v1.Models.Account
{
    public class Accounts
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public Guid JointAccountId { get; set; }
        public virtual User.User User{ get; set; }
        
    }
}
