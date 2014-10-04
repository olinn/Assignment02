

namespace CoursesAPI.Models
{
    public class AssGradeDTO
    {

        public int ID { get; set; }

        public int StudentRegistrationID { get; set; }

        public int AssignmentID { get; set; }

        public float Grade { get; set; }
    }
}
