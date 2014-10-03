using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Models.ViewModels
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

    }
}
