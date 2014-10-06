
namespace CoursesAPI.Models
{
    /// <summary>
    /// A DTO object pertaining to a persons registration as a teacher
    /// </summary>
    public class TeacherRegistrationDTO
    {
        /// <summary>
        /// database id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Social security number linked to persons
        /// </summary>
        public string SSN { get; set; }
        /// <summary>
        /// Course the person is a teacher in
        /// </summary>
        public int CourseInstanceID { get; set; }
        /// <summary>
        /// Type of teacher
        /// </summary>
        public int Type { get; set; }
    }
}
