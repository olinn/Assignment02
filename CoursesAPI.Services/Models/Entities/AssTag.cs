using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Services.Models.Entities
{
    public class AssTag
    {
   

        /// <summary>
        /// Name of the tag
        /// </summary>
        [Key]
        public string AssignmentTag { get; set; }

        /// <summary>
        /// Assignment references the CourseInstance which binds semester and course together
        /// </summary>
        public int NoToGrade { get; set; }     

   
    }
}
