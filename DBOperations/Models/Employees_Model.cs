using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DBOperations.Models
{
    public class Employees_Model
    {
        [DisplayName("Employee ID")]
        public int EmployeeID { get; set; }

        [Required][DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [EmailAddress_Validation]
        public string EmailAddress { get; set; }

        public int OTP { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
        [Password_Validation][DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm the Password")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
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