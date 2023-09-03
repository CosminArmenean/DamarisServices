using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.User
{
    public class LoginRequest
    {
        public string RequestType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
