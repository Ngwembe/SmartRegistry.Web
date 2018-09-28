using System;
using System.ComponentModel.DataAnnotations;

namespace SmartRegistry.Web.Models
{
    public class Faculty
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is a required field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is a required field")]
        public string Code { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
