using Damaris.Domain.v1.Dtos.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.Outgoing
{
    /// <summary>
    /// Single item return
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        public T Content { get; set; }
        public Error? Error { get; set; }
        public bool IsSuccess => Error == null;

        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}
