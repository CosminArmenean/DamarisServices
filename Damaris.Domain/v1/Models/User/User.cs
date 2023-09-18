using Damaris.Domain.v1.Models.Culture;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.User
{
    public class User : IdentityUser
    {
        public Guid UserId { get; init; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public int CountryId { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? MobilePhone { get; init; }
        public char  Gender { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime BirthDate { get; set; }
        public int IsActive { get; set; }
        public int UserType { get; set; }
        public DateTime LastLogin { get; set; }
        public string ProfileImage { get; set; }
        public virtual Country Country { get; set; }
    }
}
