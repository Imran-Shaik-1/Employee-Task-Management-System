using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DBOperations.Models
{
    public class Tasks_Model
    {
        [DisplayName("Task ID")]
        public int TaskID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Status { get; set; }

        [Required][DisplayName("Assigned User Story ID")]
        public int AssignedUserStoryID { get; set; }

        [DisplayName("Created On")]
        public System.DateTime CreatedOn { get; set; }

        [DisplayName("Modified On")]
        public System.DateTime ModifiedOn { get; set; }

        public virtual UserStories_Model UserStories { get; set; }
    }
}