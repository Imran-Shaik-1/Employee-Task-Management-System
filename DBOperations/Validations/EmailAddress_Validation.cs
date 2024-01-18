using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DBOperations.Models
{
    class EmailAddress_Validation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string email = value.ToString();
                if (email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    return ValidationResult.Success;
                }
            }     

            ErrorMessage = ErrorMessage ?? "Invalid Email Address. Email should end with @gmail.com";
            return new ValidationResult(ErrorMessage);
        }
    }
}
