using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Damaris.Domain.v2.Utilities.Validators
{

    /// <summary>
    /// Password validator
    /// </summary>
    public class PasswordValidator
    {
        /// <summary>
        /// This method check if string contains the elements required for a password
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public bool ValidatePassword(string value)
        {

            if (!string.IsNullOrWhiteSpace(value))
            {
                Regex? hasNumber = new(@"[0-9]+");
                Regex? hasUpperChar = new(@"[A-Z]+");
                Regex? hasSpecialCharacter = new(@"[#?!@$%^&*-]");
                //Regex? hasMinimum8Chars = new(@".{7,}");
                //checking if string contains a digit, an uppr letter, a special character and the length is minimum 8 characters.
                bool isValidated = hasNumber.IsMatch(value) && hasUpperChar.IsMatch(value) && hasSpecialCharacter.IsMatch(value);
                if (isValidated)
                {
                    //do something
                }
                else
                {
                    //do something else

                }
            }
            else
            {
                //
            }

            return false;
        }
    }
}
