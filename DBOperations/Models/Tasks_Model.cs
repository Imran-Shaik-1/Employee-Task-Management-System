using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DBOperations.Models
{
    public class Tasks_Model
    {
        public int TaskID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Status { get; set; }

        [Required]
        public int AssignedUserStoryID { get; set; }

        public System.DateTime CreatedOn { get; set; }
        
        public System.DateTime ModifiedOn { get; set; }

        public virtual UserStories_Model UserStories { get; set; }
    }
}