﻿namespace CoursesAPI.Services.Models.Entities
{
    public class StudentRegistration
    {
        /// <summary>
        /// Auto-generated ID from database
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// SSN linked to a Person SSS
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// Student i signed into a specific course of a specific semester
        /// </summary>
        public int CourseInstanceID { get; set; }

        
    }
}