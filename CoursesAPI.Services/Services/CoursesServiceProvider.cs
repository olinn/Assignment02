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
        /// <summary>
        /// Returns a list of specific courses teachers
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Add a list of students to a course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<StudentRegistrationDTO> AddListOfStudents(int courseInstanceID, List<AddStudentViewModel> model)
        {
            List<StudentRegistrationDTO> studentList = new List<StudentRegistrationDTO>();
            foreach(AddStudentViewModel i in model)
            {
                studentList.Add(AddStudent(courseInstanceID,i));
            }

            return studentList;

        }
        /// <summary>
        /// Adds a student to a specific course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public StudentRegistrationDTO AddStudent(int courseInstanceID, AddStudentViewModel model)
        {
            //Course must exist
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);
            //SSN of student must exist
            var person =  _persons.GetPerson(model.SSN);

            if(courseInstanceID != model.CourseInstanceID)
            {
                throw new AppObjectNotFoundException("CourseInstanceIDs do not match");
            }
            if(person != null && course != null)
            {
                StudentRegistration sr = new StudentRegistration
                {
                    CourseInstanceID = courseInstanceID,
                    SSN = model.SSN,
                    Status = model.Status,
                    UserName = model.UserName
                };
                _studentRegistrations.Add(sr);
                _uow.Save();
            }

            return new StudentRegistrationDTO {
                CourseInstanceID = model.CourseInstanceID,
                SSN = model.SSN,
                Status = model.Status,
                UserName = model.UserName
            };
        }
        /// <summary>
        /// Returns a list of a specific courses students person info
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns a list of a specific courses student registrations
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
        public List<StudentRegistration> GetCourseStudentRegistrations(int courseInstanceID)
        {
            _courseInstances.GetCourseInstanceByID(courseInstanceID);

            return (from t in _studentRegistrations.All()
                          where t.CourseInstanceID == courseInstanceID
                          && t.Status == 1
                          select t).ToList();        
        }
        /// <summary>
        /// Returns course instances on a specific semester
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public List<CourseInstanceDTO> GetCourseInstancesOnSemester(string semester)
        {

            return (from c in _courseInstances.All()
                    join t in _teacherRegistrations.All() on c.ID equals t.CourseInstanceID
                    join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
                    join p in _persons.All() on t.SSN equals p.SSN
                    where c.SemesterID == semester
                    && t.Type == 1
                    select new CourseInstanceDTO
                    {
                        CourseInstanceID = c.ID,
                        CourseID = c.CourseID,
                        Name = ct.Name,
                        MainTeacher = p.Name
                    }).ToList();         
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
                Tag = model.Tag,
                Required = model.Required
            };

            _assignments.Add(ass);
            _uow.Save();
           

            return new AssignmentDTO
            {
                Name = model.Name,
                Description = model.Description,
                Percentage = model.Percentage,
                CourseInstanceID = courseInstanceID,
                Tag = model.Tag ,
                Required = model.Required
            }; 
            
        }

        /// <summary>
        /// Add Assignment Tag to database
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public AssTagDTO AddAssignmentTag(int courseInstanceID, AddAssignmentTagViewModel model)
        {
            //Business rule 0: Operations on a course must use a valid course ID.
            _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Tag cannot already exist
            var assignmentTag = _assignmentTags.GetAssignmentTag(model.Name);
            if (assignmentTag != null)
            {
                //Business rule 2: Tag must exist in AssignmentTag table
                throw new AppObjectNotFoundException("Assignment Tag already  exist!");
            }

         
            AssTag assT = new AssTag
            {
                Name = model.Name,
                NoToGrade = model.NoToGrade,
                CourseInstanceID = courseInstanceID
            };
            _assignmentTags.Add(assT);
            _uow.Save();


            return new AssTagDTO
            {
                Name = model.Name,
                NoToGrade = model.NoToGrade,
                CourseInstanceID = courseInstanceID
            };       
        }

        public List<AssTagDTO> GetAssignmentTags(int courseInstanceID)
        {

            //Business rule 0: Operations on a course must use a valid course ID.
            _courseInstances.GetCourseInstanceByID(courseInstanceID);

             List<AssTagDTO> assTagList = (
                from a in _assignmentTags.All()
                where a.CourseInstanceID == courseInstanceID
                select new AssTagDTO {
                    Name = a.Name,
                    NoToGrade = a.NoToGrade,
                    CourseInstanceID = a.CourseInstanceID
                }
                ).ToList();

             return assTagList;
        }

        /// <summary>
        /// Teacher: Grade an assignment for a specific student
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Teacher: Get all grades for an assignment
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        public List<GradeListDTO> GetAllGradesOnAssignment(int courseInstanceID, int assignmentID)
        {
            //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Assignment must exist
            var assignment = _assignments.GetAssignmentByID(assignmentID);

            var students = _studentRegistrations.All();

           // var grades = _assignmentGrades.All().Where(c => c.AssignmentID == assignmentID);
      
            //magic
            List<GradeListDTO> assignmentGrades = 
                 (
                          from t in _studentRegistrations.All()
                          join p in _persons.All() on t.SSN equals p.SSN
                          join r in _assignmentGrades.All() on t.ID equals r.StudentRegistrationID
                          join x in _assignments.All() on r.AssignmentID equals x.ID
                          where t.CourseInstanceID == courseInstanceID
                          && r.AssignmentID == assignmentID
                          && t.Status == 1
                          select new GradeListDTO {
                            StudentName = p.Name,
                            StudentRegistrationID = t.ID,
                            AssignmentName = x.Name,
                            AssignmentID = r.AssignmentID,
                            CourseInstanceID = x.CourseInstanceID,
                            Percentage = x.Percentage,
                            Grade = r.Grade
                          }
                          ).ToList();

            assignmentGrades.OrderBy(n => n.StudentName);

            return assignmentGrades;
        }
        
        
        /// <summary>
        /// Student: Get grade for a single assignment
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public AssGradeDTO GetGradeFromAssignment(int courseInstanceID, int assignmentID, int studentID)
        {

            //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Assignment must exist
            var assignment = _assignments.GetAssignmentByID(assignmentID);

            //Business rule 3: Student must exist
            var student = _studentRegistrations.GetStudentRegistration(studentID);

            var studentPerson = _persons.GetPerson(student.SSN);

            var grade = _assignmentGrades.All().SingleOrDefault(c => c.AssignmentID == assignmentID && c.StudentRegistrationID == studentID);

            List<GradeListDTO> allGrades = GetAllGradesOnAssignment(courseInstanceID, assignmentID);

            double place = allGrades.OrderByDescending(g => g.Grade).ToList().FindIndex(n => n.StudentRegistrationID == studentID) + 1;           

            double average = allGrades.Sum(n => n.Grade) / allGrades.Count();
           

            return new AssGradeDTO {
                StudentRegistrationID = grade.StudentRegistrationID,
                StudentName = studentPerson.Name,
                AssignmentName = assignment.Name,
                AssignmentID = grade.AssignmentID,
                Percentage = assignment.Percentage,
                Grade = grade.Grade,
                NumberInClass = place,
                Average = average
            };

        }
        /// <summary>
        ///  Get grades for all assignments in specific course for a specific Student
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public List<GradeListDTO> GetAllSingleStudentGrades(int courseInstanceID,int studentID)
        {

             //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);

            //Business rule 1: Student must exist
            var student = _studentRegistrations.GetStudentRegistration(studentID);

            //magic
            List<GradeListDTO> assignmentGrades =
                 (
                          from g in _assignmentGrades.All()
                          join a in _assignments.All() on g.AssignmentID equals a.ID                       
                          where a.CourseInstanceID == courseInstanceID    
                          && g.StudentRegistrationID == studentID
                          select new GradeListDTO
                          {
                              StudentRegistrationID = g.StudentRegistrationID,
                              AssignmentName = a.Name,
                              AssignmentID = a.ID,
                              AssignmentTag = a.Tag,
                              CourseInstanceID = courseInstanceID,
                              Required = a.Required,
                              Percentage = a.Percentage,
                              Grade = g.Grade
                          }
                          ).ToList();
            return assignmentGrades;
        }

        /// <summary>
        /// Returns the final grade of one studento
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public FinalGradeDTO GetFinalGradeForSingleStudent(int courseInstanceID, int studentID)
        {
            return GetFinalGradesForAllStudents(courseInstanceID)
                .Where(s => s.StudentRegistrationID == studentID)
                .SingleOrDefault();
        }
        /// <summary>
        /// Get final grade for all students in a course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
        public List<FinalGradeDTO> GetFinalGradesForAllStudents(int courseInstanceID)
        {
            List<FinalGradeDTO> classFinalGrade = new List<FinalGradeDTO>();

            List<StudentRegistration> studs = GetCourseStudentRegistrations(courseInstanceID);

            foreach (StudentRegistration s in studs)
            {
                classFinalGrade.Add(GetFinalGrade(courseInstanceID, s.ID));
            }            

            classFinalGrade.OrderByDescending(g => g.Grade);
            int index = 1;
            int gradeCount = classFinalGrade.Count();
            double classAverageGrade = classFinalGrade.Sum(s => s.Grade) / gradeCount;

            foreach (FinalGradeDTO f in classFinalGrade)
            {
                f.MyRanking = index + " / " + gradeCount;
                f.ClassAverage = classAverageGrade;
                index++;
            }

            return classFinalGrade;
        }

        /// <summary>
        ///  Get final grade for single student
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        public FinalGradeDTO GetFinalGrade(int courseInstanceID, int studentID)
        {
            //Business rule 0: Operations on a course must use a valid course ID.
            var course = _courseInstances.GetCourseInstanceByID(courseInstanceID);
            var courseTemplate = _courseTemplates.GetCourse(course.CourseID);

            //Business rule 1: Student must exist
            var student = _studentRegistrations.GetStudentRegistration(studentID);
            var studentPerson = _persons.GetPerson(student.SSN);

            List<GradeListDTO> allStudentGrades = GetAllSingleStudentGrades(courseInstanceID, studentID);            
            
            List<AssTag> allTagsForCourse = _assignmentTags.GetAssignmentTags(courseInstanceID);

            List<GradeListDTO> trimmedGrades = new List<GradeListDTO>();

            double finalGrade = 0;
            double totalPercentage = 0;
            double assignmentsPercTotal = 0;
            bool coursePassed = true;
            bool readyToGrade = true;
          
            //Go over all tags and pick the amount of items that should be counted, depending on grade.
            foreach(AssTag i in allTagsForCourse)
            {
               //List<GradeListDTO> tempItems =  
                trimmedGrades.AddRange(allStudentGrades.Where(g => g.AssignmentTag == i.Name).OrderByDescending(g => g.Grade).Take(i.NoToGrade).ToList());        
            }
            //take those who dont have a tag
            foreach(GradeListDTO i in allStudentGrades)
            {
                if(i.AssignmentTag == null || i.AssignmentTag == "")
                {
                    trimmedGrades.Add(i);
                }
            }

            //Work through our list of grades we use for final
            foreach(GradeListDTO i in trimmedGrades)
            {
                finalGrade += i.Grade * i.Percentage / 100;
                totalPercentage += i.Percentage;

                if(i.Required == true && i.Grade < 4.75)
                {
                    coursePassed = false;
                }              
            }
            

            //Grade should be in even or .5 numbers.
            finalGrade = Math.Round(finalGrade * 2, MidpointRounding.AwayFromZero) / 2;

            if(finalGrade < 4.75 * totalPercentage/100)
                coursePassed = false;

            if (finalGrade > 10)
                finalGrade = 10;

            

            //The course does not have assignments graded to 100%
            if(totalPercentage < 100)
            {
                finalGrade = 0;
                readyToGrade = false;
                coursePassed = false;
            }

            return new FinalGradeDTO
            {
                StudentRegistrationID = student.ID,
                StudentName = studentPerson.Name,
                CourseName = courseTemplate.Name,
                CourseID = course.CourseID,
                PercentageReady = totalPercentage,
                ReadyToGrade = readyToGrade,
                Grade = finalGrade,
                Passed = coursePassed
            };
        }

       
      
	}
}
