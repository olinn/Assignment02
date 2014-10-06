namespace CoursesAPI.Models
{
	/// <summary>
	/// DTO object for returning Course Instances
	/// </summary>
	public class CourseInstanceDTO
	{
        /// <summary>
        /// Course instance ID
        /// </summary>
		public int    CourseInstanceID { get; set; }
        /// <summary>
        /// Course ID
        /// </summary>
		public string CourseID         { get; set; }
        /// <summary>
        /// Course name
        /// </summary>
		public string Name             { get; set; }
        /// <summary>
        /// Course teacher
        /// </summary>
		public string MainTeacher      { get; set; }
	}
}
