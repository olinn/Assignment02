using System.ComponentModel.DataAnnotations;

namespace CoursesAPI.Models
{
    public class AddAssignmentTagViewModel
    {
        /// <summary>
        /// Identification tag for groups
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Number of assignments to use
        /// </summary>
        [Required]
        public int NoToGrade { get; set; }

        [Required]
        public int CourseInstanceID { get; set; }

    }
}
