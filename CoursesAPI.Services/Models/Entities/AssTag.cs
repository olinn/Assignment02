using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Services.Models.Entities
{
    public class AssTag
    {

        public int ID { get; set; }
        /// <summary>
        /// Name of the tag
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// Assignment references the CourseInstance which binds semester and course together
        /// </summary>
        public int NoToGrade { get; set; }

        public int CourseInstanceID { get; set; }

   
    }
}
