using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.User
{
    public class JwtClaims
    {
        public string Subject { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpiresIn { get; set; }

        public DateTime IssuedAt { get; set; }


        public Dictionary<string, string> CustomClaims { get; set; } = new Dictionary<string, string>();

        public JwtClaims()
        {
            IssuedAt = DateTime.UtcNow;
        }

        public JwtClaims(string subject, string issuer, string audience, int expiresIn)
        {
            Subject = subject;
            Issuer = issuer;
            Audience = audience;
            ExpiresIn = expiresIn;
            IssuedAt = DateTime.UtcNow;
        }
    }
}
