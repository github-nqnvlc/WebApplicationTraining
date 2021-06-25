using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationTraining.Models;
using System.Data.Entity;
using WebApplicationTraining.ViewModels;
using System.Data.Entity.Migrations;

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
            
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            var courses = _context.Courses.ToList();

            var TrainerTopicVM = new TraineeCourseViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourses = new TraineeCourse()
            };

            return View(TrainerTopicVM);
        }

        [HttpPost]
        public ActionResult Create(TraineeCourseViewModel model)
        {
            
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            

            var courses = _context.Courses.ToList();


            if (ModelState.IsValid)
            {
                _context.TraineeCourses.Add(model.TraineeCourses);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var TraineeCourseVM = new TraineeCourseViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourses = new TraineeCourse()
            };

            return View(TraineeCourseVM);
        }

        [HttpGet]
        [Authorize(Roles ="TrainingStaff")]
        public ActionResult Edit(int id)
        {
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            var courses = _context.Courses.ToList();
            var traineecourseId = _context.TraineeCourses.SingleOrDefault(n => n.Id == id);
            if (traineecourseId == null) return HttpNotFound();
            var viewModel = new TraineeCourseViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourses = new TraineeCourse()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "TrainingStaff")]
        public ActionResult Edit(TraineeCourseViewModel model)
        {
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            var courses = _context.Courses.ToList();

            if (!ModelState.IsValid) return View();
            var TraineeCourseVM = new TraineeCourseViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourses = new TraineeCourse()
            };
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}