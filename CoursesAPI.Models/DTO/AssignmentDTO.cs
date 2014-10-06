namespace CoursesAPI.Models
{
    /// <summary>
    /// DTO object to return a Assignment to frontend
    /// </summary>
    public class AssignmentDTO
    {
        /// <summary>
        /// assignment name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Assignment description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Course instance id
        /// </summary>
        public int CourseInstanceID { get; set; }
        /// <summary>
        /// Assignment tag
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Percentage value of total course grade
        /// </summary>
        public double Percentage { get; set; }
        /// <summary>
        /// Is this assignment required to pass?
        /// </summary>
        public bool Required { get; set; }
    }
}
