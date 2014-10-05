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

            var courseInstances = new List<CourseInstance>
            {
                new CourseInstance
                {
                    ID = 1,
                    CourseID = "T-514-VEFT",
                    SemesterID = "20143"
                }
            };
            _uow.SetRepositoryData(courseInstances);

            var assTags = new List<AssTag>
            {
                new AssTag
                {
                    AssignmentTag = "ass",
                    NoToGrade = 5
                }
            };
            _uow.SetRepositoryData(assTags);

            AddAssignmentTagViewModel newAssTag = new AddAssignmentTagViewModel
            {
                AssignmentTag = "newAss",
                NumberOfAssignments = 5
            };

            _service.AddAssignmentTag(courseInstances[0].ID, newAssTag);

            AddAssignmentViewModel assVM = new AddAssignmentViewModel
            {
                Description = "newCourse",
                Name = "mosO",
                Percentage = 20.0,
                Tag = "newAss"
            };
            _service.AddAssignmentOnCourse(courseInstances[0].ID, assVM);


        }

        [TestMethod]
        public void CheckAddAssTag()
        {
            const int courseInstanceID = 1;

            var assTagList = new List<AssTag> { };
            _uow.SetRepositoryData(assTagList);

            AddAssignmentTagViewModel assTagModel = new AddAssignmentTagViewModel
            {
                AssignmentTag = "ass",
                NumberOfAssignments = 5
            };

            AssTagDTO assTDTO = _service.AddAssignmentTag(courseInstanceID, assTagModel);

            Assert.AreEqual(assTagModel.AssignmentTag, assTDTO.AssignmentTag);
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
            Assert.AreEqual(assVM.Tag, assDTO.Tag);
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
        [ExpectedExceptionWithMessage(typeof (ArgumentException),
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
