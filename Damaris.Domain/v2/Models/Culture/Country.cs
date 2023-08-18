using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v2.Models.Culture
{
    public record Country
    {
        public string? Code { get; init; }

        public string? Name { get; init; }

        public int? PhoneCode { get; init; }
    }
}

