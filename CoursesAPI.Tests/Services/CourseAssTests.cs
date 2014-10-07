using System;
using System.Runtime;
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
    public class CourseAssTests
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
            _uow.SetRepositoryData(TestExtensions.GetAssTag());

            AddAssignmentTagViewModel newAssTag = new AddAssignmentTagViewModel
            {
                Name = "newAss",
                NoToGrade = 5,
                CourseInstanceID = 1
            };

            _service.AddAssignmentTag(1, newAssTag);

            AddAssignmentViewModel assVM = new AddAssignmentViewModel
            {
                Description = "newCourse",
                Name = "mosO",
                Percentage = 20.0,
                Tag = "newAss"
            };
            _service.AddAssignmentOnCourse(1, assVM);


        }

        [TestMethod]
        public void CheckAddAssTag()
        {
            const int courseInstanceID = 1;

            var assTagList = new List<AssTag> { };
            _uow.SetRepositoryData(assTagList);

            AddAssignmentTagViewModel assTagModel = new AddAssignmentTagViewModel
            {
                Name = "ass",
                NoToGrade = 5,
                CourseInstanceID = 1
            };

            AssTagDTO assTDTO = _service.AddAssignmentTag(courseInstanceID, assTagModel);

            Assert.AreEqual(assTagModel.Name, assTDTO.Name, "AssTag added and asstag queried dont match");
        }

        [TestMethod]
        public void CheckAddAssOnCourse()
        {
            const int courseInstanceID = 1;

            AddAssignmentViewModel assVM = new AddAssignmentViewModel
            {
                Description = "osomCourse",
                Name = "Osom",
                Percentage = 20.0,
                Tag = "ass"
            };

            AssignmentDTO assDTO = _service.AddAssignmentOnCourse(courseInstanceID, assVM);
            Assert.AreEqual(assVM.Tag, assDTO.Tag, "AssTag added to course and then queried dont match");
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof (AppObjectNotFoundException),
            "Assignment Tag does not exist, please create it before creating assignment!")]
        public void CheckAddingAssignmentOnNonExistingtag()
        {
            //Arrange:
            const int courseInstanceID = 1;

            AddAssignmentViewModel assVM = new AddAssignmentViewModel
            {
                Description = "thisCourse",
                Name = "thisCourse",
                Percentage = 20.0,
                Tag = "noAss"
            };

            //Act:
            AssignmentDTO assDTO = _service.AddAssignmentOnCourse(courseInstanceID, assVM);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException),
            "Assignment name already exists")]
        public void CheckAddingAssignmentWithExistingName()
        {
            //Arrange:
            const int courseInstanceID = 1;

            AddAssignmentViewModel assVM = new AddAssignmentViewModel
            {
                Description = "osomCourse",
                Name = "mosO",
                Percentage = 20.0,
                Tag = "newAss"
            };

            //Act:
            _service.AddAssignmentOnCourse(courseInstanceID, assVM);
            
        }
    }
}
