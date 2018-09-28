using System.ComponentModel.DataAnnotations;

namespace SmartRegistry.Web.ViewModels.MailsViewModel
{
    public class MailSenderViewModel
    {
        //public string From { get; set; }
        //public string To { get; set; }
        //public string ToAddress { get; set; }
        //public MailboxAddress FromMailboxAddress { get; set; }
        //public MailboxAddress ToMailboxAddress { get; set; }

        [Required]
        [Display(Name="Sender Name")]
        public string SenderName { get; set; }
        [Required]
        [Display(Name = "Sender Email Address")]
        public string SenderEmailAddress { get; set; }
        [Required]
        [Display(Name = "Recipient Name")]
        public string RecipientName { get; set; }
        [Required]
        [Display(Name = "Recipient Email Address")]
        public string RecipientEmailAddress { get; set; }
        [Required]
        public string Subject { get; set; }
        //public IList<string> BCCs { get; set; }
        public string Bcc { get; set; }

        //public MimeMessage MimeMessage { get; set; } = new MimeMessage();
        [Required]
        [Display(Name="Body")]
        public string Content { get; set; }
    }
}
