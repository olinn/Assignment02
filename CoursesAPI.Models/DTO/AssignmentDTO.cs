namespace CoursesAPI.Models
{
    public class AssignmentDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CourseInstanceID { get; set; }

        public string AssTag { get; set; }
    }
}
