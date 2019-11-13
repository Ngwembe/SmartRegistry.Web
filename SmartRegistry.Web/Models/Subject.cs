﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRegistry.Web.Models
{
    [Table("Subject")]
    public class Subject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SubjectId { get; set; }
        
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

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        
        [Required(ErrorMessage = "{0} is a required field")]
        public int LecturerId { get; set; }
        public virtual Lecturer Lecturer { get; set; }
    }
}
