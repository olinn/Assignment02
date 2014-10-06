using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services;

namespace CoursesAPI.Controllers
{
	[RoutePrefix("api/courses")]
	public class CoursesController : ApiController
	{
		private readonly CoursesServiceProvider _service;

		public CoursesController()
		{
			_service = new CoursesServiceProvider(new UnitOfWork<AppDataContext>());
		}

        /// <summary>
        /// Returns teachers on a course instance
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
		[Route("{courseInstanceID:int}/teachers")]
		public List<PersonDTO> GetCourseTeachers(int courseInstanceID)
		{
			return _service.GetCourseTeachers(courseInstanceID);
		}
		/// <summary>
		/// Returns courses on a semester
		/// </summary>
		/// <param name="semester"></param>
		/// <returns></returns>
		[Route("semester/{semester}")]
		public List<CourseInstanceDTO> GetCoursesOnSemester(string semester)
		{
			return _service.GetCourseInstancesOnSemester(semester);
		}


        /////////////////////////////////////////////////////////////////////////
        //////////////////Teacher functions//////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////


        [HttpPost]
        [Route("{courseInstanceID:int}/studentslist")]
        public IHttpActionResult AddListOfStudents(int courseInstanceID, List<AddStudentViewModel> model)
        {
            if(!ModelState.IsValid ||model == null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var result = _service.AddListOfStudents(courseInstanceID, model);
                return Created("Students successfully created", result);
            }
        }
        /// <summary>
        /// Add a student to a specific course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{courseInstanceID:int}/student")]
        public IHttpActionResult AddStudent(int courseInstanceID, AddStudentViewModel model)
        {
            if(!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var result = _service.AddStudent(courseInstanceID, model);
                return Created("Student succesfully created", result);
            }
        }
        /// <summary>
        /// Teacher: Add new assignment
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{courseInstanceID:int}/assignment/")]
        public  IHttpActionResult AddAssignmentOnCourse(int courseInstanceID, AddAssignmentViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                
                var result = _service.AddAssignmentOnCourse(courseInstanceID, model);
                return Created("Assignment succesfully created", result);                           
            }
                
        }

        /// <summary>
        /// Teacher: Add new Assignment TAG
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{courseInstanceID:int}/tags")]
        public IHttpActionResult AddAssignmentTag(int courseInstanceID, AddAssignmentTagViewModel model)
        {
            if(!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var result = _service.AddAssignmentTag(courseInstanceID, model);
                return Created("Tag succesfully added", result);
            }
        }
        /// <summary>
        /// Returns all tags for a specific course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{courseInstanceID:int}/tags")]
        public IHttpActionResult GetAssignmentTags(int courseInstanceID)
        { 
            var result = _service.GetAssignmentTags(courseInstanceID);
            return Created("Tags succesfully retrieved", result);
        }

        /// <summary>
        /// Teacher: Grade a specific Assignment
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{courseInstanceID:int}/assignment/{assignmentID:int}/")]
        public IHttpActionResult AddGrade(int courseInstanceID, int assignmentID, AddGradeViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Created("Grade added", _service.AddGradeToAssignment(courseInstanceID, assignmentID, model));
            }            
        }
        /// <summary>
        /// Teacher: Get all grades for a specific assignment
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{courseInstanceID:int}/assignment/{assignmentID:int}/grades")]
        public IHttpActionResult GetAllGradesOnAssignment(int courseInstanceID, int assignmentID)
        {

            return Ok(_service.GetAllGradesOnAssignment(courseInstanceID, assignmentID));

        }

        /// <summary>
        /// Returns final grades for all students in a specific course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{courseInstanceID:int}/grades")]
        public IHttpActionResult GetFinalGrades(int courseInstanceID)
        {
            return Ok(_service.GetFinalGradesForAllStudents(courseInstanceID));
        }


        /////////////////////////////////////////////////////////////////////////
        //////////////////Student functions//////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Get grade for single assignment for single student in a specific course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// /// <param name="assignmentID"></param>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{courseInstanceID:int}/student/{studentID:int}/assignment/{assignmentID:int}/grade")]
        public IHttpActionResult GetGrade(int courseInstanceID, int assignmentID, int studentID)
        {
            var result = _service.GetGradeFromAssignment(courseInstanceID, assignmentID, studentID);

            return Ok(result);
        }
        /// <summary>
        /// Returns all grades for specific student in a specific course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{courseInstanceID:int}/student/{studentID:int}/grades")]
        public IHttpActionResult GetAllGrades(int courseInstanceID, int studentID)
        {
            var result = _service.GetAllSingleStudentGrades(courseInstanceID, studentID);

            return Ok(result);
        }
        /// <summary>
        /// Returns final grade for specific student in a specific course
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="studentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{courseInstanceID:int}/student/{studentID:int}/finalgrade")]
        public IHttpActionResult GetFinalGrades(int courseInstanceID, int studentID)
        {
            var result = _service.GetFinalGradeForSingleStudent(courseInstanceID, studentID);

            return Ok(result);
        }

      


	}
}
