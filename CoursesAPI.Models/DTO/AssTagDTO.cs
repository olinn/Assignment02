

namespace CoursesAPI.Models
{
    /// <summary>
    /// DTO for Assignment Tag
    /// </summary>
    public class AssTagDTO
    {
        /// <summary>
        /// Assignment tag/name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Number of assignments with this tag to grade
        /// </summary>
        public int NoToGrade { get; set; }
        /// <summary>
        /// Course instance id
        /// </summary>
        public int CourseInstanceID { get; set; }


    }
}
