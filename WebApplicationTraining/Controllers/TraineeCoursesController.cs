using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationTraining.Models;
using System.Data.Entity;
using WebApplicationTraining.ViewModels;

namespace WebApplicationTraining.Controllers
{
    public class TraineeCoursesController : Controller
    {
        private ApplicationDbContext _context;

        public TraineeCoursesController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            if (User.IsInRole("TrainingStaff"))
            {
                var traineecourses = _context.TraineeCourses.Include(t => t.Course).Include(t => t.Trainee).ToList();
                return View(traineecourses);
            }
            if (User.IsInRole("Trainee"))
            {
                var traineeId = User.Identity.GetUserId();
                var Res = _context.TraineeCourses.Where(e => e.TraineeId == traineeId).Include(t => t.Course).ToList();
                return View(Res);
            }
            return View("Login");
        }
        [HttpGet]
        [Authorize(Roles = "TrainingStaff")]

        public ActionResult Delete(int id)
        {
            var courseInDb = _context.TraineeCourses.SingleOrDefault(p => p.Id == id);

            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            _context.TraineeCourses.Remove(courseInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            //get trainer
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            //get topic

            var courses = _context.Courses.ToList();

            var TrainerTopicVM = new TraineeCourseViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourse = new TraineeCourse()
            };

            return View(TrainerTopicVM);
        }

        [HttpPost]
        public ActionResult Create(TraineeCourseViewModel model)
        {
            //get trainer
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            //get topic

            var courses = _context.Courses.ToList();


            if (ModelState.IsValid)
            {
                _context.TraineeCourses.Add(model.TraineeCourse);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var TrainerTopicVM = new TraineeCourseViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourse = new TraineeCourse()
            };

            return View(TrainerTopicVM);
        }
    }
}