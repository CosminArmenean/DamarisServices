using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Damaris.Domain.v1.Dtos.GenericDtos
{
    public class Result<T>
    {
        public T? Content { get; set; }
        public Error? Error { get; set; }
        public bool IsSuccess => Error == null;
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}
