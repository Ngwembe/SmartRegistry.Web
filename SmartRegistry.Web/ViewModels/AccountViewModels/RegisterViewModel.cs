using System;
using System.ComponentModel.DataAnnotations;

namespace SmartRegistry.Web.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Is Student")]
        public bool IsStudent { get; set; }
        
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "{0} is a required field")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "{0} is a required field")]
        public string LastName { get; set; }

        [MaxLength(6)]
        [Required(ErrorMessage = "{0} is a required field and characters must not be greater than six")]
        public string Gender { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "{0} is a required field")]
        public DateTime DOB { get; set; }
    }
}
