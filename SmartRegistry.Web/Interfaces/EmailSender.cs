using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace SmartRegistry.Web.Interfaces
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly MailboxAddress _systemMailboxAddress;

        public EmailSender()
        {
            _systemMailboxAddress = new MailboxAddress("Smart Attendance", "smartattendance45@gmail.com");
        }

        public async Task HandleConfirmationEmailSendAsync(string email, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(_systemMailboxAddress);
            message.To.Add(new MailboxAddress("User", email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body //$"{body}"
            };

            using (var client = new SmtpClient())
            {
                await  client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("smartattendance45@gmail.com", "Coder@18");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return;// Task.CompletedTask;
        }

        public async Task<Task> SendEmailAsync(string recipientName, string recipientEmailAddress, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(_systemMailboxAddress);
            message.To.Add(new MailboxAddress(recipientName, recipientEmailAddress));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body //$"{body}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("smartattendance45@gmail.com", "Coder@18");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }


            return Task.CompletedTask;
        }
    }
}
