using Damaris.Domain.v1.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.Requests.Account
{
    public class JointAccountRequestDto
    {
        public RegisterUserDto User { get; set; }
        public RegisterUserDto JointUser { get; set; }
    }
}
