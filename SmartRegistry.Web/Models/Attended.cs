using System;
using System.ComponentModel;

namespace SmartRegistry.Web.Models
{
    public class Attended
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "{0} is a required field")]
        [DisplayName("Is Present")]
        public bool HasAttended { get; set; } = false;

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;

        [DisplayName("Student Name")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }


        /*
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        [DisplayName("Is Present")]
        public bool HasAttended { get; set; } = false;

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public int DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "{0} is a required field")]
        [DisplayName("Student Name")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        [DisplayName("Subject Name")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        */
    }
}
