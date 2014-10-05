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
                throw new ArgumentException("Assignment name already exists");
            return null;
        }

        public static AssTag GetAssignmentTag(this IRepository<AssTag> repo, string tag)
        {
            var assignTag = repo.All().SingleOrDefault(c => c.AssignmentTag == tag);

            return assignTag;
        }
            
        
    }
}
