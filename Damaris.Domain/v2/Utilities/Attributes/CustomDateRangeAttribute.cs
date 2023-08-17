using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v2.Utilities.Attributes
{
    /// <summary>
    /// Custom date range attribute
    /// </summary>
    public class CustomDateRangeAttribute : RangeAttribute
    {
        /// <summary>
        /// Constructor for date range attribute 
        /// Setting min value to 110 years back and max value to 13 years back
        /// </summary>
        public CustomDateRangeAttribute() : base(typeof(DateTime), DateTime.Now.AddYears(-110).ToString("dd-MM-yyyy"), DateTime.Now.AddYears(-13).ToString("dd-MM-yyyy")) { }
    }
}
