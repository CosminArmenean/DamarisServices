using Damaris.Domain.v1.Models.Account;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.User
{
    public class LoginResponse
    {
        public string EventType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }
        public string IpAddress { get; set; }
        public bool Success { get; set; }
        public UserRole? UserRoles { get; set; }
        //public Location Locations { get; set; }
    }
}
