

namespace CoursesAPI.Models
{
    public class AssGradeDTO
    {
        /// <summary>
        /// Student ID
        /// </summary>
        public int StudentRegistrationID { get; set; }
        /// <summary>
        /// Name of student
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Assignment of ID
        /// </summary>
        public int AssignmentID { get; set; }

        /// <summary>
        /// Name of assignment
        /// </summary>
        public string AssignmentName { get; set; }

        /// <summary>
        /// Grade for assignment
        /// </summary>
        public double Grade { get; set; }

        /// <summary>
        /// Assignment percentage value of final grade
        /// </summary>
        public double Percentage { get; set; }
        /// <summary>
        /// What number in class this grade is, not always applicable
        /// </summary>
        public double NumberInClass { get; set; }

        /// <summary>
        /// Average for this assignment
        /// </summary>
        public double Average { get; set; }
       
    }
}
