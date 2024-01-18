using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DBOperations.Models
{
    public class UserStories_Model
    {
        public int UserStoryID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]

        public System.DateTime CreatedOn { get; set; }

        [Required]
        public int AssignedEmployeeID { get; set; }

        public int AssignedManagerID { get; set; }

        [DataType(DataType.Date)][DueDate_Validation]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DueDate { get; set; }

        public System.DateTime ModifiedOn { get; set; }

        public string Status { get; set; }

        [Range(0, 10, ErrorMessage = "StoryPoints must be between 0 and 10.")]
        [Required]
        public int StoryPoints { get; set; }

        [Required]
        public string Priority { get; set; }

        public virtual Employees_Model Employees { get; set; }

        public virtual ICollection<Tasks_Model> Tasks { get; set; }
    }
}