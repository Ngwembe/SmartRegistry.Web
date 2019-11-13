using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRegistry.Web.Models
{
    [Table("Lecturer")]
    public class Lecturer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LecturerId { get; set; }

        public string AccountId { get; set; }
        [MaxLength(6)]
        [Required(ErrorMessage = "{0} is a required field and characters must not be greater than six")]
        public string Gender { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is a required field")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is a required field")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        public int Age { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "{0} is a required field")]
        public DateTime DOB { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;

        //public virtual List<Subject> Subject { get; set; }
    }
}
