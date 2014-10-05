namespace CoursesAPI.Models
{
    public class AssignmentDTO
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public int CourseInstanceID { get; set; }

        public string Tag { get; set; }

        public double Percentage { get; set; }

        public bool Required { get; set; }
    }
}
