using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRegistry.Web.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        [Display(Name = "Lecture Room")]
        public string LectureRoom { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        [Display(Name = "Is Confirmed")]
        public bool IsConfirmed { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        [Display(Name = "Scheduled From")]
        public DateTime ScheduleFor { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        [Display(Name = "Scheduled To")]
        public DateTime ScheduleTo { get; set; }

        //[Required(ErrorMessage = "{0} is a required field")]
        [Display(Name = "Color Theme")]
        public string ColorTheme { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "{0} is a required field")]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
