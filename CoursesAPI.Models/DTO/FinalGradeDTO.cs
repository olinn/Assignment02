using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    /// <summary>
    /// DTO object to return a final grade
    /// </summary>
    public class FinalGradeDTO
    {
        /// <summary>
        /// Student Registration ID
        /// </summary>
        public int StudentRegistrationID { get; set; }
        /// <summary>
        /// Student Name
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// Name of course
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// ID-Name of Course
        /// </summary>
        public string CourseID { get; set; }
        /// <summary>
        /// Percentage of final grade complete/ready
        /// </summary>
        public double PercentageReady { get; set; }
        /// <summary>
        /// Does the course have assignments that total at 100%?
        /// </summary>
        public bool ReadyToGrade { get; set; }
        /// <summary>
        /// Final grade
        /// </summary>
        public double Grade { get; set; }
        /// <summary>
        /// Did I just pass this shit?
        /// </summary>
        public bool Passed { get; set; }
        /// <summary>
        /// Class Average
        /// </summary>
        public double ClassAverage { get; set; }
        /// <summary>
        /// Place in class ranking in grades
        /// </summary>
        public string MyRanking { get; set; }



    }
}
