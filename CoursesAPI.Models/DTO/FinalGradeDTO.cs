using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesAPI.Models
{
    public class FinalGradeDTO
    {
        public int StudentRegistrationID { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }

        public double PercentageReady { get; set; }

        public double Grade { get; set; }

    }
}
