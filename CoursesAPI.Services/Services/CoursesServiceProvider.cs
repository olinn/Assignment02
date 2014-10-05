using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
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
        private readonly IRepository<StudentRegistration> _studentRegistrations;
        private readonly IRepository<AssGrade> _assignmentGrades;

        public CoursesServiceProvider(IUnitOfWork uow)
        {
            _uow = uow;

            _courseInstances = _uow.GetRepository<CourseInstance>();
            _courseTemplates = _uow.GetRepository<CourseTemplate>();
            _teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
            _persons = _uow.GetRepository<Person>();
            _studentRegistrations = _uow.GetRepository<StudentRegistration>();
            _assignments = _uow.GetRepository<Assignment>();
            _assignmentTags = _uow.GetRepository<AssTag>();
            _assignmentGrades = _uow.GetRepository<AssGrade>();

        }

        public List<PersonDTO> GetCourseTeachers(int courseInstanceID)
        {
            _courseInstances.GetCourseInstanceByID(courseInstanceID);

            var result = (from t in _teacherRegistrations.All()
                          join p in _persons.All() on t.SSN equals p.SSN
                          where t.CourseInstanceID == courseInstanceID
                          select p).ToList();

            List<PersonDTO> teachers = new List<PersonDTO>();

            foreach (Person p in result)
            {
                PersonDTO pDTO = new PersonDTO
                {
                    ID = p.ID,
                    Name = p.Name,
                    SSN = p.SSN
                };
                teachers.Add(pDTO);
            }
            return teachers;
        }

        public List<PersonDTO> GetCourseStudents(int courseInstanceID)
        {
            _courseInstances.GetCourseInstanceByID(courseInstanceID);

            var result = (from t in _studentRegistrations.All()
                          join p in _persons.All() on t.SSN equals p.SSN
                          where t.CourseInstanceID == courseInstanceID
                          && t.Status == 1
                          select p).ToList();

            List<PersonDTO> students = new List<PersonDTO>();

            foreach (Person p in result)
            {
                PersonDTO pDTO = new PersonDTO
                {
                    ID = p.ID,
                    Name = p.Name,
                    SSN = p.SSN
                };
                students.Add(pDTO);
            }
            return students;
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
            _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Assignment name is unique, must not be already existing
            var assignment = _assignments.All().SingleOrDefault(c => c.Name == model.Name);
            if (assignment != null)
            {
                throw new AppObjectNotFoundException("Assignment name already exists");
            }
                

            var assignmentTag = _assignmentTags.GetAssignmentTag(model.Tag);
            
            if(model.Tag != null && assignmentTag == null)
            {
                //Business rule 2: Tag must exist in AssignmentTag table
                throw new AppObjectNotFoundException("Assignment Tag does not exist, please create it before creating assignment!");
            }

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
            _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Tag cannot already exist
            var assignmentTag = _assignmentTags.GetAssignmentTag(model.AssignmentTag);

            if (assignmentTag != null)
            {
                //Business rule 2: Tag must exist in AssignmentTag table
                throw new AppObjectNotFoundException("Assignment Tag already  exist!");
            }

            AssTag assT = new AssTag
            {
                AssignmentTag = model.AssignmentTag,
                NoToGrade = model.NumberOfAssignments,
                CourseInstanceID = courseInstanceID
            };
            _assignmentTags.Add(assT);
            _uow.Save();


            return new AssTagDTO
            {
                AssignmentTag = model.AssignmentTag,
                NoToGrade = model.NumberOfAssignments,
                CourseInstanceID = courseInstanceID
            };           

        }

        public AssGradeDTO AddGradeToAssignment(int courseInstanceID, int assignmentID, AddGradeViewModel model)
        {

            //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);            

            if(assignmentID != model.AssignmentID)
                throw new AppObjectNotFoundException("AssignmentID mismatch in path and model.");

            //Business rule 1: Assignment must exist
            var assignment = _assignments.GetAssignmentByID(assignmentID);

            //CourseInstanceID in path must be the same as in the assignment being graded
            if (courseInstanceID != assignment.CourseInstanceID)
                throw new AppObjectNotFoundException("CourseInstanceID does not match assignment CourseInstanceID");

            //Business rule 3: Student must exist
            var student = _studentRegistrations.GetStudentRegistration(model.StudentRegistrationID);

            if(assignment != null && student != null)
            {
                var assG = new AssGrade
                {
                    StudentRegistrationID = model.StudentRegistrationID,
                    AssignmentID = model.AssignmentID,
                    Grade = model.Grade
                };

                _assignmentGrades.Add(assG);
                _uow.Save();                
            }

            return new AssGradeDTO
            {
                StudentRegistrationID = model.StudentRegistrationID,
                AssignmentID = model.AssignmentID,
                Grade = model.Grade
            };
        }
     

        //Student functions

        public AssGradeDTO GetGradeFromAssignment(int courseInstanceID, int assignmentID, int studentID)
        {

            //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Assignment must exist
            var assignment = _assignments.GetAssignmentByID(assignmentID);

            //Business rule 3: Student must exist
            var student = _studentRegistrations.GetStudentRegistration(studentID);

            var grade = _assignmentGrades.All().SingleOrDefault(c => c.AssignmentID == assignmentID && c.StudentRegistrationID == studentID);

            return new AssGradeDTO {
                StudentRegistrationID = grade.StudentRegistrationID,
                AssignmentID = grade.AssignmentID,
                Grade = grade.Grade
            };

        }

      
	}
}
