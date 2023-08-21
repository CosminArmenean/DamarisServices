using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.Account
{
    public class UserToken
    {
        public string? UserId { get; set; }
        public string? LoginProvider { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
