using Damaris.Domain.v1.Utilities.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Damaris.Domain.v1.Utilities.Attributes;

namespace Damaris.Domain.v1.Models.User
{
    /// <summary>
    /// Register user model dto
    /// </summary>
    public record RegisterUser
    {
        [Required(ErrorMessage = "RegisterTwoUser field is required.")]
        [Display(Name = "Register Two User")]
        public bool RegisterTwoUser { get; init; }

        [Required(ErrorMessage = "The First Name field is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [MaxLength(30)]
        [Display(Name = "First Name")]
        public string? FirstName { get; init; }

        [Required(ErrorMessage = "The Last Name field is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [MaxLength(30)]
        [Display(Name = "Last Name")]
        public string? LastName { get; init; }

        [Required(ErrorMessage = "The password field is required.")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 7)]
        [Display(Name = "Password")]
        //[BindProperty(BinderType = typeof(IntToBoolModelBinder))]
        public string? Password { get; init; }

        [Required(ErrorMessage = "The password field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password does not match.")]
        public string? PasswordConfirmation { get; init; } = string.Empty;

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        [Display(Name = "Email")]
        public string? Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Confirm email")]
        [Compare("Email", ErrorMessage = "The email and email confirmation do not match.")]
        public string? EmailConfirmation { get; init; } = string.Empty;

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(3)]
        [Display(Name = "Country")]
        public string? CountryCode { get; init; }

        [Required(ErrorMessage = "The mobile phone field is required.")]
        [Phone]
        [Display(Name = "Mobile phone")]
        public string? MobilePhone { get; init; }

        [Required(ErrorMessage = "The Birth Date field is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        [CustomDateRange(ConvertValueInInvariantCulture = true)]
        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; init; }

        [Required(ErrorMessage = "The Gender field is required.")]
        [StringRange(AllowableValues = new[] { "M", "F" }, ErrorMessage = "Gender must be either 'M' or 'F'")]
        [Display(Name = "Gender")]
        public string? Gender { get; init; }

        public DateTime RegisteredAt { get; init; }
        public RegisterUser? LinkedUser { get; init; }
    }
}
