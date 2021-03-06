﻿using System;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Tests.MockObjects;
using System.Collections.Generic;
using CoursesAPI.Tests.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoursesAPI.Models;
using CoursesAPI.Services.Models.Entities;


namespace CoursesAPI.Tests.Services
{
    [TestClass]
    public class CourseStudentTests
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
        public void CheckStudentsInCourseWhenThereIsNone()
        {
            // Arrange:
            const int courseInstanceID = 1;

            var studentRegistration = new List<StudentRegistration> { };
            _uow.SetRepositoryData(studentRegistration);
            // Act:

            var students = _service.GetCourseStudents(courseInstanceID);

            // Assert:
            Assert.AreEqual(0, students.Count, "There is a student registered when there shouldnt");
        }

        [TestMethod]
        public void CheckStudentsInCourse()
        {
            // Arrange:
            const int courseInstanceID = 1;

            var studentRegistration = new List<StudentRegistration>
            {
                new StudentRegistration
                {
                    //ID = 1,
                    SSN = "2008814519",
                    CourseInstanceID = 1,
                    Status = 1
                }
            };
            _uow.SetRepositoryData(studentRegistration);
            // Act:

            var result = _service.GetCourseStudents(courseInstanceID);

            // Assert:
            Assert.AreEqual(1, result.Count, "There is no teacher registered when there should be");
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException), "Course Instance not found!")]
        public void GetListOfStudentsWhenCourseInstanceIDIsInvalid()
        {
            //Arrange
            const int courseInstanceID = 2;

            //Act
            _service.GetCourseStudents(courseInstanceID);

        }
    }
}
