using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.Requests.Account
{
    public class JointAccountUserDto
    {
        public Guid JointAccountUserId { get; set; }
        public Guid UserId { get; set; }
        public Guid JointAccountId { get; set; }
    }
}
