using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.Extensions;

namespace CoursesAPI
{
	public class CoursesServiceProvider
	{
		private readonly IUnitOfWork _uow;

		private readonly IRepository<CourseInstance> _courseInstances;
		private readonly IRepository<TeacherRegistration> _teacherRegistrations;
		private readonly IRepository<CourseTemplate> _courseTemplates; 
		private readonly IRepository<Person> _persons;
        private readonly IRepository<Assignment> _assignments;
        private readonly IRepository<AssTag> _assignmentTags;

		public CoursesServiceProvider(IUnitOfWork uow)
		{
			_uow = uow;

			_courseInstances      = _uow.GetRepository<CourseInstance>();
			_courseTemplates      = _uow.GetRepository<CourseTemplate>();
			_teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
			_persons              = _uow.GetRepository<Person>();
            _assignments          = _uow.GetRepository<Assignment>();
		}

		public List<Person> GetCourseTeachers(int courseInstanceID)
		{
			// TODO:
		    var result = (from t in _teacherRegistrations.All()
		        join p in _persons.All() on t.SSN equals p.SSN
		        where t.CourseInstanceID == courseInstanceID
		        select p).ToList();
			return result;
		}

		public List<CourseInstanceDTO> GetCourseInstancesOnSemester(string semester)
		{
			// TODO:
			return null;
		}

		public List<CourseInstanceDTO> GetSemesterCourses(string semester)
		{
			// TODO
			return null;
		}

        /// <summary>
        /// Add assignment to course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public AssignmentDTO AddAssignmentOnCourse(int courseInstanceID, AddAssignmentViewModel model)
        {

            //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Assignment name is unique, must not be already existing
            var assignment = _assignments.All().SingleOrDefault(c => c.Name == model.Name);
            if (assignment != null)
                throw new ArgumentException("Assignment name already exists");

            var assignmentTag = _assignmentTags.GetAssignmentTag(model.Tag);
            if(model.Tag != null && assignmentTag == null)
            {
                //Business rule 2: Tag must exist in AssignmentTag table
                throw new ArgumentException("Assignment Tag does not exist, please create it before creating assignment!");
            }
            else
            {
                //Create new assignment and save
                Assignment ass = new Assignment
                {
                    Name = model.Name,
                    Description = model.Description,
                    Percentage = model.Percentage,
                    CourseInstanceID = courseInstanceID,
                    Tag = model.Tag
                };

                _assignments.Add(ass);
                _uow.Save();
            }

            //Return new AssignmentDTO
            return new AssignmentDTO
            {
                Name = model.Name,
                Description = model.Description,
                Percentage = model.Percentage,
                CourseInstanceID = courseInstanceID,
                Tag = model.Tag 
            }; 
            
        }

        public AssTagDTO AddAssignmentTag(int courseInstanceID, AddAssignmentTagViewModel model)
        {
            //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);

            return null;

            

        }

      

      
	}
}
