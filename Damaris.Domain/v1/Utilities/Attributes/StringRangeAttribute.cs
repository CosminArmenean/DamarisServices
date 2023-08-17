using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Utilities.Attributes
{
    /// <summary>
    /// Custom string ranges attribute
    /// </summary>
    public class StringRangeAttribute : ValidationAttribute
    {
        public string[]? AllowableValues { get; set; }
        /// <summary>
        /// This method is checking if value contains one of the values from AllowableValues array
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (AllowableValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }
            string message = $"Please enter one of the allowable values: {string.Join(", ", (AllowableValues ?? new string[] { "No allowable values found" }))}.";
            return new ValidationResult(message);
        }
    }
}

