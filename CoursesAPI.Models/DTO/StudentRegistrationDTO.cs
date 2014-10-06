namespace CoursesAPI.Models
{
    /// <summary>
    /// A DTO object pertaining to a persons registration as a student
    /// </summary>
    public class StudentRegistrationDTO
    {
        /// <summary>
        /// database id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Social security number of person registered
        /// </summary>
        public string SSN { get; set; }
        /// <summary>
        /// ID of course instance 
        //
        /// </summary>
        public int CourseInstanceID { get; set; }
        /// <summary>
        /// Status, 1 = active, 0 = inactive
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Student's username for web
        /// </summary>
        public string UserName { get; set; }
    }
}
