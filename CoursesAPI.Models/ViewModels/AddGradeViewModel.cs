using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Models
{
    public class AddGradeViewModel
    {
        /// <summary>
        /// ID of student who the grade belongs to
        /// </summary>
        [Required]
        public int StudentRegistrationID { get; set; }

        /// <summary>
        /// ID of assignment 
        /// </summary>
        [Required]
        public int AssignmentID { get; set; }

        /// <summary>
        /// Student's grade for assignment
        /// </summary>
        public double Grade { get; set; }
    }
}
