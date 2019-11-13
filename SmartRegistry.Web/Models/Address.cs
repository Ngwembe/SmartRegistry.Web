using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRegistry.Web.Models
{
    [Table("Address")]
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AddressId { get; set; }

        [Display(Name = "Residential Address")]
        [Required(ErrorMessage ="{0} is a required field")]
        public string Residential { get; set; }

        [Display(Name = "Postal Address")]
        [Required(ErrorMessage = "{0} is a required field")]
        public string Postal { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "{0} is a required field")]
        public string PostalCode { get; set; }

        [Display(Name = "Is Address For Student, Lecturer Or Office")]
        [Required(ErrorMessage = "{0} is a required field")]
        public AddressType AddressType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public string DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }

    public enum AddressType
    {
        Office = 0,
        Lecturer = 1,
        Student = 2
    }
}
