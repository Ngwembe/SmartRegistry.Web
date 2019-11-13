using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRegistry.Web.Models
{
    [Table("Announcement")]
    public class Announcement
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AnnouncementId { get; set; }

        public string Title { get; set; }

        [DisplayName("Publisher")]
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }

        [DisplayName("Announcement Body")]
        [Required(ErrorMessage = "{0} is required")]
        public string Message { get; set; }

        [DisplayName("Announcement Type")]
        [Required(ErrorMessage = "{0} is required")]
        public int AnnouncementTypeId { get; set; }
        //public AnnouncementType AnnouncementType { get; set; }

        [DisplayName("Announcement Belongs To")]
        [Required(ErrorMessage = "{0} is required")]
        public int AnnouncementTypeOwnerId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }

    //public enum AnnouncementType
    //{
    //    Faculty = 1,
    //    Department = 2,
    //    Course = 3,
    //    Subject = 4
    //}

    public class AnnouncementType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        //public int Value { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }

}
