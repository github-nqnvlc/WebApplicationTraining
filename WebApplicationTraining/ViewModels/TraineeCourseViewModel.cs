using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationTraining.Models;

namespace WebApplicationTraining.ViewModels
{
    public class TraineeCourseViewModel
    {
        public TraineeCourse TraineeCourse { get; set; }
        public IEnumerable<ApplicationUser> Trainees { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}