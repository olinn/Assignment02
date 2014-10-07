using System;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Tests.MockObjects;
using System.Collections.Generic;
using CoursesAPI.Tests.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoursesAPI.Models;
using CoursesAPI.Services.Models.Entities;

namespace CoursesAPI.Tests.Services
{
    public static class TestExtensions
    {
        public static List<Person> GetPerson()
        {
            var persons = new List<Person>
		    {
		        new Person
		        {
		            ID = 1,
		            SSN = "2008814519",
		            Name = "Marinó",
		            Email = "marino12@ru.is"
		        }

		    };
            return persons;
        }

        public static List<CourseInstance> GetCourseInstance()
        {
            var courseInstances = new List<CourseInstance>
            {
                new CourseInstance
                {
                    ID = 1,
                    CourseID = "T-514-VEFT",
                    SemesterID = "20143"
                }
            };
            return courseInstances;
        }

        public static List<AssTag> GetAssTag()
        {
            var assTags = new List<AssTag>
            {
                new AssTag
                {
                    ID = 1,
                    Name = "ass",
                    NoToGrade = 5,
                    CourseInstanceID = 1
                }
            };
            return assTags;
        }

        public static List<StudentRegistration> GetStudent()
        {
            var students = new List<StudentRegistration>
            {
                new StudentRegistration
                {
                    ID = 1,
                    SSN = "2008814519",
                    CourseInstanceID = 1,
                    Status = 1
                }
            };

            return students;
        }

        public static List<CourseTemplate> GetCourseTemplates()
        {
            var cTemplates = new List<CourseTemplate>
            {
                new CourseTemplate
                {
                    CourseID = "T-514-VEFT",
                    Name = "Vefþjónustur",
                    Description = "Apar og allskonar"

                }
            };
            return cTemplates;
        }

    }
}
