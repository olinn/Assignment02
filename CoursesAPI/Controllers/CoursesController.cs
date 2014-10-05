﻿using System.Collections.Generic;
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


		[Route("{courseInstanceID:int}/teachers")]
		public List<PersonDTO> GetCourseTeachers(int courseInstanceID)
		{
			return _service.GetCourseTeachers(courseInstanceID);
		}
		
		[Route("semester/{semester}")]
		public List<CourseInstanceDTO> GetCoursesOnSemester(string semester)
		{
			return _service.GetSemesterCourses(semester);
		}


        /////////////////////////////////////////////////////////////////////////
        //////////////////Teacher functions//////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Teacher: Add new assignment
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{courseInstanceID:int}/addAssignment/")]
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
        [Route("{courseInstanceID:int}/addTag")]
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
        /// Teacher: Grade a specific Assignment
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{courseInstanceID:int}/assignment/{assignmentID:int}/addGrade")]
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
        [Route("{courseInstanceID:int}/assignment/{assignmentID:int}/getGrades")]
        public IHttpActionResult GetGrades(int courseInstanceID, int assignmentID)
        {

            return Ok(_service.GetAllGradesOnAssignment(courseInstanceID, assignmentID));

        }


        /////////////////////////////////////////////////////////////////////////
        //////////////////Student functions//////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Needs authentication to return 
        /// </summary>
        /// <param name="courseInstanceID"></param>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{courseInstanceID:int}/student/{studentID:int}/assignment/{assignmentID:int}/getGrade")]
        public IHttpActionResult GetGrade(int courseInstanceID, int assignmentID, int studentID)
        {
            var result = _service.GetGradeFromAssignment(courseInstanceID, assignmentID, studentID);

            return Ok(result);
        }

        [HttpGet]
        [Route("{courseInstanceID:int}/student/{studentID:int}/getAllGrades")]
        public IHttpActionResult GetAllGrades(int courseInstanceID, int studentID)
        {
            var result = _service.GetAllSingleStudentGrades(courseInstanceID, studentID);

            return Ok(result);
        }

      


	}
}
