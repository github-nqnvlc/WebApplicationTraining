using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplicationTraining.Models;
using WebApplicationTraining.ViewModels;
using static WebApplicationTraining.Controllers.AccountController;
using static WebApplicationTraining.Controllers.ManageController;

namespace WebApplicationTraining.Controllers
{
    public class ManageUsersController : Controller
    {
        private ApplicationDbContext _context;
        public ManageUsersController()
        {
            _context = new ApplicationDbContext();
        }
        
        public ActionResult UsersWithRoles()
        {
            var usersWithRoles = (from user in _context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Name = user.Name,
                                      Age = user.Age,
                                      WorkingPlace = user.WorkingPlace,
                                      Username = user.UserName,
                                      Emailaddress = user.Email,
                                      Password = user.PasswordHash,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in _context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new Users_In_Role()

                                  {
                                      UserId = p.UserId,
                                      Name = p.Name,
                                      Username = p.Username,
                                      Email = p.Emailaddress,
                                      Role = string.Join(",", p.RoleNames)
                                  });


            return View(usersWithRoles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var appUser = _context.Users.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ApplicationUser user)
        {
            var userInDb = _context.Users.Find(user.Id);

            if (userInDb == null)
            {
                return View(user);
            }

            if (ModelState.IsValid)
            {
                userInDb.Name = user.Name;
                userInDb.UserName = user.UserName;
                userInDb.Age = user.Age;
                userInDb.WorkingPlace = user.WorkingPlace;
                userInDb.Phone = user.Phone;
                userInDb.Email = user.Email;


                _context.Users.AddOrUpdate(userInDb);
                _context.SaveChanges();

                return RedirectToAction("UsersWithRoles");
            }
            return View(user);
        }
        [Authorize(Roles = "Admin, Trainer")]
        public ActionResult Details(string id)
        {
            if (id == null) return HttpNotFound();
            var userDetail = _context.Users.SingleOrDefault(t => t.Id == id);
            if (userDetail == null) return HttpNotFound();
            return View(userDetail);
        }
        [Authorize(Roles = "Admin,Trainer,Trainee,TrainingStaff")]
        public ActionResult Profile()
        {
            var userProfileId = User.Identity.GetUserId();
            if (userProfileId == null) return HttpNotFound();
            var userProfile = _context.Users.SingleOrDefault(p => p.Id == userProfileId);
            return View(userProfile);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Trainer, Trainee,TrainingStaff")]
        public ActionResult EditProfile(string id)
        {
            id = User.Identity.GetUserId();
            if (id == null) return HttpNotFound();
            var userInfo = _context.Users.Find(id);
            if (userInfo == null) return HttpNotFound();
            return View(userInfo);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Trainer, Trainee,TrainingStaff")]
        public ActionResult EditProfile(ApplicationUser user)
        {
            user.Id = User.Identity.GetUserId();
            var userProfile = _context.Users.Find(user.Id);
            if (userProfile == null) return View(user);
            if (ModelState.IsValid)
            {
                userProfile.Name = user.Name;
                userProfile.UserName = user.UserName;
                userProfile.Age = user.Age;
                userProfile.WorkingPlace = user.WorkingPlace;
                userProfile.Phone = user.Phone;
                userProfile.Email = user.Email;
                _context.Users.AddOrUpdate(userProfile);
                _context.SaveChanges();

                return RedirectToAction("Profile");
            }
            return View(user);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            var userInDb = _context.Users.SingleOrDefault(p => p.Id == id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(userInDb);
            _context.SaveChanges();

            return RedirectToAction("UsersWithRoles");

        }
    }
}