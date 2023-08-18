using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Damaris.Domain.v2.Utilities.Generic
{
    /// <summary>
    /// Convert integer to bool value
    /// </summary>
    public class IntToBoolModelBinder
    {
        public bool IntegerToBoolean(string value)
        {           
            if (int.TryParse(value, out var intValue))
            {
               //do something

            }
            else if (bool.TryParse(value, out var boolValue))
            {
                //do something else

            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                //dunno

            }
            else
            {
                // dunno
            }

            return false;
        }
    }
}
