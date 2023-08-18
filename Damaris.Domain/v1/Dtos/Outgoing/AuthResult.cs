using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.Outgoing
{
    public record AuthResult
    {
        public string? Token { get; init; }
        public string? RefreshToken { get; set; }
        public bool Success { get; init; }
        public List<string>? Errors { get; set; }
    }
}

