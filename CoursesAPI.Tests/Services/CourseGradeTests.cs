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
    public class CourseGradeTests
    {
        private CoursesServiceProvider _service;
        private MockUnitOfWork<MockDataContext> _uow;
        private AddGradeViewModel addGradeVM;

        [TestInitialize]
        public void Setup()
        {
            _uow = new MockUnitOfWork<MockDataContext>();
            _service = new CoursesServiceProvider(_uow);

            _uow.SetRepositoryData(TestExtensions.GetPerson());
            _uow.SetRepositoryData(TestExtensions.GetCourseInstance());
            _uow.SetRepositoryData(TestExtensions.GetAssTag());
            _uow.SetRepositoryData(TestExtensions.GetStudent());

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

            List<Assignment> assignmentList = new List<Assignment>();
            var assignment = new Assignment
            {
                CourseInstanceID = 1,
                Description = "foo",
                ID = 1,
                Name = "foobar",
                Percentage = 20.0,
                Required = true,
                Tag = "newAss"
            };
            assignmentList.Add(assignment);
            _uow.SetRepositoryData(assignmentList);

            addGradeVM = new AddGradeViewModel
            {
                StudentRegistrationID = 1,
                AssignmentID = 1,
                Grade = 8.0
            };

            _service.AddGradeToAssignment(1, 1, addGradeVM);
        }

        // ***************************************
        //            Testing AddGradeToAssignment
        // ***************************************

        [TestMethod]
        public void CheckNewGradeToAssignment()
        {
            //Arrange:

            int courseInstanceID = 1;
            int assignmentID = 2;

            List<Assignment> assignmentList = new List<Assignment>();
            var assignment = new Assignment
            {
                CourseInstanceID = courseInstanceID,
                Description = "foo",
                ID = 2,
                Name = "foobar",
                Percentage = 20.0,
                Required = true,
                Tag = "newAss"
            };
            assignmentList.Add(assignment);
            _uow.SetRepositoryData(assignmentList);

            var addGradeVM = new AddGradeViewModel
            {
                StudentRegistrationID = 1,
                AssignmentID = assignmentID,
                Grade = 9.0
            };

            //Act:
            AssGradeDTO newGrade = _service.AddGradeToAssignment(courseInstanceID, assignmentID, addGradeVM);

            //Assert:
            Assert.AreEqual(addGradeVM.Grade, newGrade.Grade);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException), "Course Instance not found!")]
        public void CheckNewGradeToNonExistingCourse()
        {
            //Arrange:

            int courseInstanceID = 2;
            int assignmentID = 2;

            List<Assignment> assignmentList = new List<Assignment>();
            var assignment = new Assignment
            {
                CourseInstanceID = courseInstanceID,
                Description = "foo",
                ID = 2,
                Name = "foobar",
                Percentage = 20.0,
                Required = true,
                Tag = "newAss"
            };
            assignmentList.Add(assignment);
            _uow.SetRepositoryData(assignmentList);

            var addGradeVM = new AddGradeViewModel
            {
                StudentRegistrationID = 1,
                AssignmentID = assignmentID,
                Grade = 9.0
            };

            //Act:
            AssGradeDTO newGrade = _service.AddGradeToAssignment(courseInstanceID, assignmentID, addGradeVM);

        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException), "AssignmentID mismatch in path and model.")]
        public void CheckNewGradeWhenAssignmentIDInAssignmentDoesNotMatchGivenAssignmentID()
        {
            //Arrange:

            int courseInstanceID = 1;
            int assignmentID = 2;

            List<Assignment> assignmentList = new List<Assignment>();
            var assignment = new Assignment
            {
                CourseInstanceID = courseInstanceID,
                Description = "foo",
                ID = 2,
                Name = "foobar",
                Percentage = 20.0,
                Required = true,
                Tag = "newAss"
            };
            assignmentList.Add(assignment);
            _uow.SetRepositoryData(assignmentList);

            var addGradeVM = new AddGradeViewModel
            {
                StudentRegistrationID = 1,
                AssignmentID = 1,
                Grade = 9.0
            };

            //Act:
            AssGradeDTO newGrade = _service.AddGradeToAssignment(courseInstanceID, assignmentID, addGradeVM);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException), "CourseInstanceID does not match assignment CourseInstanceID")]
        public void CheckNewGradeWhenCourseIDInAssignmentDoesNotMatchGivenCourseID()
        {
            //Arrange:

            int courseInstanceID = 1;
            int assignmentID = 2;

            List<Assignment> assignmentList = new List<Assignment>();
            var assignment = new Assignment
            {
                CourseInstanceID = 2,
                Description = "foo",
                ID = 2,
                Name = "foobar",
                Percentage = 20.0,
                Required = true,
                Tag = "newAss"
            };
            assignmentList.Add(assignment);
            _uow.SetRepositoryData(assignmentList);

            var addGradeVM = new AddGradeViewModel
            {
                StudentRegistrationID = 1,
                AssignmentID = 2,
                Grade = 9.0
            };

            //Act:
            AssGradeDTO newGrade = _service.AddGradeToAssignment(courseInstanceID, assignmentID, addGradeVM);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException), "Assignment does not exist")]
        public void CheckNewGradeWhenAssignmentDoesNotExist()
        {
            //Arrange:

            int courseInstanceID = 1;
            int assignmentID = 3;

            List<Assignment> assignmentList = new List<Assignment>();
            var assignment = new Assignment
            {
                CourseInstanceID = 1,
                Description = "foo",
                ID = 2,
                Name = "foobar",
                Percentage = 20.0,
                Required = true,
                Tag = "newAss"
            };
            assignmentList.Add(assignment);
            _uow.SetRepositoryData(assignmentList);

            var addGradeVM = new AddGradeViewModel
            {
                StudentRegistrationID = 1,
                AssignmentID = 3,
                Grade = 9.0
            };

            //Act:
            AssGradeDTO newGrade = _service.AddGradeToAssignment(courseInstanceID, assignmentID, addGradeVM);
        }

        // *******************************************
        //            Testing GetAllGradesOnAssignment
        // *******************************************

        [TestMethod]
        public void CheckAllGrades()
        {
            //Arrange:
            int courseInstanceID = 1;
            int assignmentID = 1;   

            //Act:
            List<GradeListDTO> allGrades = _service.GetAllGradesOnAssignment(courseInstanceID, assignmentID);

            //Assert:
            Assert.AreEqual(1, allGrades.Count);
        }

        // *******************************************
        //            Testing GetAllGradesOnAssignment
        // *******************************************

        // GetGradeFromAssignment

        [TestMethod]
        public void CheckGettingGradeFromAssignment()
        {
            //Arrange:
            int courseInstanceID = 1;
            int assignmentID = 1;
            int studentID = 1;

            //Act:
            AssGradeDTO studentGrades = _service.GetGradeFromAssignment(courseInstanceID, assignmentID, studentID);

            //Assert:
            Assert.AreEqual(addGradeVM.Grade, studentGrades.Grade);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(AppObjectNotFoundException), "Student is not registered")]
        public void CheckGettingGradeFromAssignmentWithInvalidStudent()
        {
            //Arrange:
            int courseInstanceID = 1;
            int assignmentID = 1;
            int studentID = 2;

            //Act:
            AssGradeDTO studentGrades = _service.GetGradeFromAssignment(courseInstanceID, assignmentID, studentID);
        }


    }
}
