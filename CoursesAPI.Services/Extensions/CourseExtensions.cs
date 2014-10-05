using System;
using System.Collections.Generic;
using System.Linq;

using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;

namespace CoursesAPI.Services.Extensions
{
    public static class CourseExtensions
    {
        /// <summary>
        /// Get A course by ID
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CourseInstance GetCourseInstanceByID(this IRepository<CourseInstance> repo, int id)
        {

            var course = repo.All().SingleOrDefault(c => c.ID == id);

            if (course == null)
                throw new AppObjectNotFoundException("Course Instance not found!");

            return course;
        }

        public static Assignment GetAssignmentByName(this IRepository<Assignment> repo, string name)
        {
            var assignment = repo.All().SingleOrDefault(c => c.Name == name);

            if (assignment != null)
                throw new AppObjectNotFoundException("Assignment name already exists");
            return null;
        }

        public static Assignment GetAssignmentByID(this IRepository<Assignment> repo, int id)
        {
            var assignment = repo.All().SingleOrDefault(c => c.ID == id);
            if (assignment == null)
            {
                throw new AppObjectNotFoundException("Assignment does not exist");
            }
            return assignment;
            
        }

        public static AssTag GetAssignmentTag(this IRepository<AssTag> repo, string tag)
        {
            try
            {
                var assignTag = repo.All().SingleOrDefault(c => c.Name == tag);
                return assignTag;
            }
            catch
            {            
               
            }
            return null;
            
        }

        public static StudentRegistration GetStudentRegistration(this IRepository<StudentRegistration> repo, int studentID)
        {
            var student = repo.All().SingleOrDefault(c => c.ID == studentID);

            if (student == null)
                throw new AppObjectNotFoundException("Student is not registered");

            return student;
        }

            
        
    }
}
