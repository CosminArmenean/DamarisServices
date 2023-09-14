using Damaris.Domain.v1.Models.Culture;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.UserDtos
{
    public record UserDto
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? Email { get; set; }

        public DateTime BirthDate { get; init; }
        public char Gender { get; init; }

        public DateTime RegisteredAt { get; init; }

        public bool IActive { get; init; }

        public DateTime LastLogin { get; init; }

        public string? About { get; init; }

        public Country? Country { get; init; }
        public Guid? LinkedWithAccount { get; init; }

        public UserDto? UserLinked { get; init; }
    }
}
