using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MimeKit.Text;

namespace SmartRegistry.Web.Interfaces
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly MailboxAddress _systemMailboxAddress;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmailSender(IHostingEnvironment hostingEnvironment)
        {
            _systemMailboxAddress = new MailboxAddress("Smart Attendance", "smartattendance45@gmail.com");
            _hostingEnvironment = hostingEnvironment;
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
                await client.AuthenticateAsync("smartattendance45@gmail.com", "Jabulile@09");

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

        //public async Task SendReportAsync(string fileName  /*string recipientName, string recipientEmailAddress, string subject, string body*/)
        public async Task SendReportAsync(byte[] file  /*string recipientName, string recipientEmailAddress, string subject, string body*/)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                string recipientName = "Tshepo Motswiane";
                string recipientEmailAddress = "tsepo.motswiane@gmail.com";
                string subject = "Testing EMAIL WITH REPORTING DOCUMENTS";

                var attachment = new AttachmentCollection()
                {
                    { "Enrolled Student List", file }
                };

                stream.Write(file, 0, file.Length);
                
                // create an image attachment for the file located at path
                var fileAttachment = new MimePart("application/pdf")
                {
                    Content = new MimeContent(stream),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "Enrolled Student List"
                };

                // now create the multipart/mixed container to hold the message text and the
                // image attachment
                var multipart = new Multipart("mixed");
                multipart.Add(new TextPart("html")
                {
                    Text = $"Dear <b>{recipientName}</b><br/><br/>" +
                           $"Thank you for using the platform.<br/><br/>" +
                           $"We have attached a copy of the list of enrolled students to this email for your convenience.<br/><br/>" +
                           $"<br/><br/>" 
                           //+ $"<img src='http://framefun.co.za/wp-content/uploads/2017/01/COJ-Logo-1.jpeg' alt='' height='150' width='150'>"
                });

                multipart.Add(fileAttachment);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Smart Attendance", "smartattendance45@gmail.com"));
                //message.From.Add(_systemMailboxAddress);

                message.To.Add(new MailboxAddress(recipientName, recipientEmailAddress));
                message.Subject = subject;

                message.Body = multipart; // builder.ToMessageBody(); //body //$"{body}"

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("smartattendance45@gmail.com", "Jabulile@09");
                    //await client.AuthenticateAsync("tsepo@mgibagroup.com", "Jabulile@009");

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                stream.Close();
                stream.Dispose();
                return;// Task.CompletedTask;
            }
            catch (Exception ex)
            {
                stream.Close();
                stream.Dispose();
            }
        }
    }
}
