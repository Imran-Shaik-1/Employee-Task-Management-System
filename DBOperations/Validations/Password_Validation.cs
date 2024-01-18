using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBOperations.Models
{
    public class Password_Validation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string password = value.ToString();
                if (password.Any(char.IsDigit) && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(c => !char.IsLetter(c)))
                {
                    return ValidationResult.Success;
                }
            }

            ErrorMessage = ErrorMessage ?? "Password should contain at least one number, one uppercase letter, one lowercase letter, and one special character.";
            return new ValidationResult(ErrorMessage);
        }
    }
}
