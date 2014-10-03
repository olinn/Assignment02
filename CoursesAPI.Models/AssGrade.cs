namespace CoursesAPI.Models
{
    public class AssGrade
    {

        public int ID { get; set; }

        public int StudentRegistrationID { get; set; }

        public int AssignmentID { get; set; }

        public double Grade { get; set; }
    }
}
