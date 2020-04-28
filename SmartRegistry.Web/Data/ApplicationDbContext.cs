using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext//<ApplicationUser>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Lecturer> Lecturer { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Faculty> Faculty { get; set; }

        public DbSet<Attended> Attendee { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<EnrolledSubject> EnrolledSubject { get; set; }

        public DbSet<Announcement> Announcement { get; set; }
        public DbSet<AnnouncementType> AnnouncementType { get; set; }

        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<Events> Log { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlServer(
        //            _configuration.GetConnectionString("AzureConnection"),
        //            options => options.EnableRetryOnFailure());

        //    //base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
