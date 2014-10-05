using CoursesAPI.Services.Exceptions;
using CoursesAPI.Tests.MockObjects;
using System.Collections.Generic;
using CoursesAPI.Tests.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoursesAPI.Services;
using CoursesAPI.Services.Models.Entities;

namespace CoursesAPI.Tests.Services
{
    [TestClass]
    public class CourseTeacherTests
    {
        private CoursesServiceProvider _service;
        private MockUnitOfWork<MockDataContext> _uow;

        [TestInitialize]
        public void Setup()
        {
            _uow = new MockUnitOfWork<MockDataContext>();
            _service = new CoursesServiceProvider(_uow);

            _uow.SetRepositoryData(TestExtensions.GetPerson());
            _uow.SetRepositoryData(TestExtensions.GetCourseInstance());

        }

        [TestMethod]
        public void CheckTeachersInCourseWhenThereIsNone()
        {
            // Arrange:
            const int courseInstanceID = 1;

            var teacherRegistrations = new List<TeacherRegistration> { };
            _uow.SetRepositoryData(teacherRegistrations);
            // Act:

            var teachers = _service.GetCourseTeachers(courseInstanceID);

            // Assert:
            Assert.AreEqual(0, teachers.Count, "There is a teacher registered when there shouldnt");
        }

        [TestMethod]
        public void CheckTeachersInCourseWhenThereIsOne()
        {
            // Arrange:
            const int courseInstanceID = 1;

            var teacherRegistrations = new List<TeacherRegistration>
            {
                new TeacherRegistration
                {
                    //ID = 1,
                    SSN = "2008814519",
                    CourseInstanceID = 1,
                    Type = 1
                }
            };
            _uow.SetRepositoryData(teacherRegistrations);
            // Act:

            var result = _service.GetCourseTeachers(courseInstanceID);

            // Assert:
            Assert.AreEqual(1, result.Count, "There is no teacher registered when there should be");
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException), "Course Instance not found!")]
        public void GetListOfTeachersWhenCourseInstanceIDIsInvalid()
        {
            //Arrange
            const int courseInstanceID = 2;

            //Act
            _service.GetCourseTeachers(courseInstanceID);

        }
    }
}
