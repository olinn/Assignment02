namespace CoursesAPI.Services.Models.Entities
{
    public class AssGrade
    {
        /// <summary>
        /// Auto-generated ID from database
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// References Student in course
        /// </summary>
        public int StudentRegistrationID { get; set; }

        /// <summary>
        /// References an assignment
        /// </summary>
        public int AssignmentID { get; set; }

        /// <summary>
        /// The student's grade
        /// </summary>
        public double Grade { get; set; }

    }
}
