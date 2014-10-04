using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Services.Models.Entities
{
    public class StudentRegistration
    {
        /// <summary>
        /// Auto-generated ID from database
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// SSN linked to a Person SSS
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// Student i signed into a specific course of a specific semester
        /// </summary>
        public int CourseInstanceID { get; set; }

        /// <summary>
        /// Status of the student. Shows if he's active or not
        /// </summary>
        public int Status { get; set; }
        
    }
}
