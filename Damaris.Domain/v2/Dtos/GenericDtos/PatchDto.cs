using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v2.Dtos.GenericDtos
{
    /// <summary>
    /// 
    /// </summary>
    public record PatchDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters is allowed.", MinimumLength = 3)]
        //[RegularExpression(@"[a-zA-Z]", ErrorMessage = "Incorrect property format")]
        [DisplayName("Property Name")]
        public string? PropertyName { get; init; }

        [Required(ErrorMessage = "This field is required.")]
        //[RegularExpression(@"[a-zA-Z0-9.-@]", ErrorMessage = "Incorrect value format")]
        [DisplayName("Property Value")]
        public object? PropertyValue { get; init; }

    }
}