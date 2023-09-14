using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.Requests.Account
{
    public class JointAccountDto
    {
        public Guid JointId { get; set; }
        // JointAccount properties
        public List<AccountDto> Accounts { get; set; }
        public List<JointAccountUserDto> JointAccountUsers { get; set; }
    }
}
