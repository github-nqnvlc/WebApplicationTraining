using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplicationTraining;
using WebApplicationTraining.ViewModels;

namespace WebApplicationTraining.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string WorkingPlace { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Phone { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<TrainerTopic> TrainerTopics { get; set; }
        public DbSet<TraineeCourse> TraineeCourses { get; set; }
        public ManagerStaffViewModel managerStaffViewModels { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}