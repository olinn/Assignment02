using System.ComponentModel.DataAnnotations;

namespace CoursesAPI.Models
{
    public class AddAssignmentTagViewModel
    {
        /// <summary>
        /// Identification tag for groups
        /// </summary>
        [Required]
        public string AssignmentTag { get; set; }

        /// <summary>
        /// Number of assignments to use
        /// </summary>
        [Required]
        public int NumberOfAssignments { get; set; }
    }
}
