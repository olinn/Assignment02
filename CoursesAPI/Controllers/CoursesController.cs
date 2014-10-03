using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Models.Entities;
using CoursesAPI.Services.Services;

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
		public List<Person> GetCourseTeachers(int courseInstanceID)
		{
			return _service.GetCourseTeachers(courseInstanceID);
		}
		
		[Route("semester/{semester}")]
		public List<CourseInstanceDTO> GetCoursesOnSemester(string semester)
		{
			return _service.GetSemesterCourses(semester);
		}

        [HttpPost]
        [Route("{courseInstanceID:int}/assignment/")]
        public  HttpResponseMessage AddAssignmentOnCourse(int courseInstanceID, AddAssignmentViewModel model)
        {

            return Request.CreateResponse(HttpStatusCode.Created, _service.AddAssignmentOnCourse(courseInstanceID, model));

        }
	}
}
