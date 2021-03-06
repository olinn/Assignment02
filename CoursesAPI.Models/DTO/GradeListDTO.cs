﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    /// <summary>
    /// DTO class to return grades
    /// </summary>
    public class GradeListDTO
    {

        /// <summary>
        /// Student name
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// Student's registration ID.
        /// </summary>
        public int StudentRegistrationID { get; set; }
        /// <summary>
        /// AssignmentID
        /// </summary>
        public int AssignmentID { get; set; }

        /// <summary>
        /// Assignment name
        /// </summary>
        public string AssignmentName { get; set; }

        /// <summary>
        /// Course instance id
        /// </summary>
        public int CourseInstanceID { get; set; }
        /// <summary>
        /// Student grade
        /// </summary>
        public double Grade { get; set; }
        /// <summary>
        /// AssignmentTag
        /// </summary>
        public string AssignmentTag { get; set; }

        /// <summary>
        /// Assignment Percentage
        /// </summary>
        public double Percentage { get; set; }
        /// <summary>
        /// Is this assignment required or not
        /// </summary>
        public bool Required { get; set; }
    }
}
