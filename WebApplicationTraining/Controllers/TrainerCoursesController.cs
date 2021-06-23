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
    public class TrainerCoursesController : Controller
    {
        private ApplicationDbContext _context;
        public TrainerCoursesController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            if (User.IsInRole("TrainingStaff"))
            {
                var trainercourses = _context.TrainerCourses.Include(t => t.Course).Include(t =>t.Trainer).ToList();
                return View(trainercourses);
            }
            if (User.IsInRole("Trainer"))
            {
                var trainerId = User.Identity.GetUserId();
                var Res = _context.TrainerCourses.Where(e => e.TrainerId == trainerId).Include(t => t.Course).ToList();
                return View(Res);
            }
            return View("Login");
        }
        [HttpGet]
        [Authorize(Roles = "TrainingStaff")]

        public ActionResult Delete(int id)
        {
            var courseInDb = _context.TrainerCourses.SingleOrDefault(p => p.Id == id);

            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            _context.TrainerCourses.Remove(courseInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {

            var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            var courses = _context.Courses.ToList();

            var TrainerCourseVM = new TrainerCourseViewModel()
            {
                Courses = courses,
                Trainers = users,
                TrainerCourse = new TrainerCourse()
            };

            return View(TrainerCourseVM);
        }

        [HttpPost]
        public ActionResult Create(TrainerCourseViewModel model)
        {
            //get trainer
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            var courses = _context.Courses.ToList();


            if (ModelState.IsValid)
            {
                _context.TrainerCourses.Add(model.TrainerCourse);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var TrainerCourseVM = new TrainerCourseViewModel()
            {
                Courses = courses,
                Trainers = users,
                TrainerCourse = new TrainerCourse()
            };

            return View(TrainerCourseVM);
        }
    }
}