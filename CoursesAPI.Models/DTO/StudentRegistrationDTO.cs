namespace CoursesAPI.Models
{
    public class StudentRegistrationDTO
    {
        public int ID { get; set; }

        public string SSN { get; set; }

        public int CourseInstanceID { get; set; }

        public int Status { get; set; }
    }
}
