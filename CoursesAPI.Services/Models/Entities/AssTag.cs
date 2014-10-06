using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Services.Models.Entities
{
    /// <summary>
    /// Entity class for assignment tags
    /// </summary>
    public class AssTag
    {
        /// <summary>
        /// Database generated ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of the tag
        /// </summary>       

        public string Name { get; set; }

        /// <summary>
        /// Assignment references the CourseInstance which binds semester and course together
        /// </summary>
        public int NoToGrade { get; set; }
        /// <summary>
        /// ID of course instance
        /// </summary>
        public int CourseInstanceID { get; set; }

   
    }
}
