using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRegistry.Web.Models
{
    [Table("Contact")]
    public class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ContactId { get; set; }
        [Required(ErrorMessage = "{0} is a required field")]
        public string CellPhone { get; set; }
        [Required(ErrorMessage = "{0} is a required field")]
        public string TelePhone { get; set; }
        [Required(ErrorMessage = "{0} is a required field")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "{0} is a required field")]
        public int UserId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
