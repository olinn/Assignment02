namespace CoursesAPI.Services.Models.Entities
{
    public class Assignment
    {
        /// <summary>
        /// A Database generated ID
        /// </summary>

        public int ID { get; set; }

        /// <summary>
        /// Name of the Assignment ex: Skilaverkefni 1
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the Assignment
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Assignment references the CourseInstance which binds semester and course together
        /// </summary>
        public int CourseInstanceID { get; set; }

        /// <summary>
        /// Assignment tag references a tag set in the AssignmentTags table
        /// </summary>
        public string AssTag { get; set; }
    }
}
