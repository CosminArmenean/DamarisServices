using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Dtos.GenericDtos
{
    public class ProducerRecord 
    {
        public string Topic { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
    }
}
