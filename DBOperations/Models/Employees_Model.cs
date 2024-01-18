using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DBOperations.Models
{
    public class Employees_Model
    {
        public int EmployeeID { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required][EmailAddress_Validation]
        public string EmailAddress { get; set; }

        public int OTP { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
        [Password_Validation][DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm the Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        
        public bool IsManager { get; set; }

        public Nullable<int> ManagerID { get; set; }

        public Nullable<int> OldManagerID { get; set; }

        public virtual ICollection<Employees_Model> Employees1 { get; set; }

        public virtual Employees_Model Employees2 { get; set; }

        public virtual ICollection<UserStories_Model> UserStories { get; set; }
    }
}