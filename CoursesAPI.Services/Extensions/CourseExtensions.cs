using System;
using System.Collections.Generic;
using System.Linq;

using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;

namespace CoursesAPI.Services.Extensions
{
    /// <summary>
    /// Functions used more than once in CourseServiceProvider
    /// Created here to fulfill DRY.
    /// </summary>
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
        /// <summary>
        /// Get assignment by name
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Assignment GetAssignmentByName(this IRepository<Assignment> repo, string name)
        {
            var assignment = repo.All().SingleOrDefault(c => c.Name == name);

            if (assignment != null)
                throw new AppObjectNotFoundException("Assignment name already exists");
            return null;
        }
        /// <summary>
        /// Get assignment by id
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Assignment GetAssignmentByID(this IRepository<Assignment> repo, int id)
        {
            var assignment = repo.All().SingleOrDefault(c => c.ID == id);
            if (assignment == null)
            {
                throw new AppObjectNotFoundException("Assignment does not exist");
            }
            return assignment;
            
        }
        /// <summary>
        /// Get assignment tag by tag
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get student registration info
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public static StudentRegistration GetStudentRegistration(this IRepository<StudentRegistration> repo, int studentID)
        {
            var student = repo.All().SingleOrDefault(c => c.ID == studentID);

            if (student == null)
                throw new AppObjectNotFoundException("Student is not registered");

            return student;
        }
        /// <summary>
        /// Get person info
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public static Person GetPerson(this IRepository<Person> repo, string ssn)
        {
            var person = repo.All().SingleOrDefault(p => p.SSN == ssn);

            if (person == null)
                throw new AppObjectNotFoundException("No person with that SSN exists.");

            return person;
        }
        /// <summary>
        /// Get a list of assignment tags
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
        public static List<AssTag> GetAssignmentTags(this IRepository<AssTag> repo, int courseInstanceID)
        {
            List<AssTag> allTags = repo.All().Where(t => t.CourseInstanceID == courseInstanceID).ToList();

            return allTags;
        }
        /// <summary>
        /// Get a course template for a specific course
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public static CourseTemplate GetCourse(this IRepository<CourseTemplate> repo, string courseID)
        {
            return repo.All().Where(c => c.CourseID == courseID).SingleOrDefault();
        }

            
        
    }
}
