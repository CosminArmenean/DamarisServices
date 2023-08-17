using Damaris.Domain.v1.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.UserDtos
{
    public record RegisterUserDto
    {
        public Guid UserId { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public int? CountryId { get; init; }
        public string? MobilePhone { get; init; }
        public DateTime BirthDate { get; init; }
        public char? Gender { get; init; }
        public string? Email { get; init; }
        public string? PasswordHash { get; init; }
        public DateTime RegisteredAt { get; init; }
        public int? IsActive { get; init; }
        public DateTime LastLogin { get; init; }
        public Guid? LinkedWithAccount { get; init; }

    }
}
