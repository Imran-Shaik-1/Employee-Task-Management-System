using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace DBOperations.Models
{
    public class DueDate_Validation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dueDate && dueDate > DateTime.Now)
            {
                return ValidationResult.Success;
            }
            ErrorMessage = ErrorMessage ?? "DueDate should be a future date.";
            return new ValidationResult(ErrorMessage);
        }
    }
}
