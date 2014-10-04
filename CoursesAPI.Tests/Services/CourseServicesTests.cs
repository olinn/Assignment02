using System;
using CoursesAPI.Tests.MockObjects;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoursesAPI.Services.Services;
using CoursesAPI.Services.Models.Entities;


namespace CoursesAPI.Tests.Services
{
	[TestClass]
	public class CourseServicesTests
	{
	    private CoursesServiceProvider _service;
	    private MockUnitOfWork<MockDataContext> _uow;
            
        [TestInitialize]
		public void Setup()
		{
            _uow = new MockUnitOfWork<MockDataContext>();
            _service = new CoursesServiceProvider(_uow);
			// TODO: code which will be executed before each test!
		}

		[TestMethod]
		public void TestMethod1()
		{
			// Arrange:
		    const int courseInstanceID = 1;

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

		    _uow.SetRepositoryData(persons);
		    var teacherRegistrations = new List<TeacherRegistration> { };
		    _uow.SetRepositoryData(teacherRegistrations);
		    // Act:

		    var result = _service.GetCourseTeachers(courseInstanceID);

		    // Assert:
            Assert.AreEqual(0, result.Count);
		}
	}
}
