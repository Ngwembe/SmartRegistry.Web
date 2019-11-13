using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRegistry.Web.Models
{
    [Table("Faculty")]
    public class Faculty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int FacultyId { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        public string Code { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
