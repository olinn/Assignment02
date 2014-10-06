using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Models
{
    public class AddStudentViewModel
    {

        /// <summary>
        /// Student SSN
        /// </summary>
        [Required]
        public string SSN { get; set; }

        /// <summary>
        /// Course ID
        /// </summary>
        [Required]
        public int CourseInstanceID { get; set; }

        /// <summary>
        /// Student status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Student's username for web
        /// </summary>
        public string UserName { get; set; }
    }
}
