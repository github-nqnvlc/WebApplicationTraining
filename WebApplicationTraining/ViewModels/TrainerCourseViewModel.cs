using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationTraining.Models;

namespace WebApplicationTraining.ViewModels
{
    public class TrainerCourseViewModel
    {
        public TrainerCourse TrainerCourse { get; set; }
        public IEnumerable<ApplicationUser> Trainers { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}