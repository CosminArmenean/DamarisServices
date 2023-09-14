using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.Account
{
    public class JointAccounts
    {
        public Guid JointId { get; set; }
        public Guid AccountId { get; set; }
        public virtual Accounts JointAccount { get; set; }
   
    }
}
