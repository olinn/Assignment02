using System.ComponentModel.DataAnnotations;
namespace CoursesAPI.Models
{
    public class AddAssignmentViewModel
    {
        /// <summary>
        /// Name of assignment
        /// </summary>
        /// 
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Description of assignment
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Percentage value of assignment
        /// </summary>
        [Required]
        public double Percentage { get; set; }


        /// <summary>
        /// Assignment TAG, to group assignments together if needed.
        /// </summary>
        public string Tag { get; set; }
    }
}
