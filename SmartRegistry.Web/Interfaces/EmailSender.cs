using System;
using System.Collections.Generic;
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

        public async Task SendReportAsync(string fileName  /*string recipientName, string recipientEmailAddress, string subject, string body*/)
        {
            try
            {
                string recipientName = "Tshepo Motswiane";
                string recipientEmailAddress = "tsepo.motswiane@gmail.com";
                string subject = "Testing EMAIL WITH REPORTING DOCUMENTS";
                //string body = "See attached document for the report.";

                //var attachment = new MimePart("application/pdf", "");

                if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                {
                    _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                var path = $"{_hostingEnvironment.WebRootPath}\\Reports\\{fileName}";

                //var attachment = new MimePart("application/pdf")
                ////var attachment = new MimePart("pdf")
                //{
                //    Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
                //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                //    ContentTransferEncoding = ContentEncoding.Base64,
                //    FileName = Path.GetFileName(path)
                //};

                var builder = new BodyBuilder();

                builder.HtmlBody = "<p>See attached document for the report.";

                //builder.Attachments.Add("testPDF", File.OpenRead(path));

                //FileStream fileStream = File.Create(path);
                //var memoryStream = new MemoryStream();
                //fileStream.Position = 0;
                //fileStream.CopyTo(memoryStream);
                //builder.Attachments.Add(new Attachment(memoryStream, Path.GetFileName(path)));
                
                builder.Attachments.Add(path);

                //attachment.Content = new MimeContent(new FileStream(path, FileMode.Open), ContentEncoding.Default);

                var message = new MimeMessage();
                message.From.Add(_systemMailboxAddress);
                message.To.Add(new MailboxAddress(recipientName, recipientEmailAddress));
                message.Subject = subject;



                //message.Attachments = new List<MimeEntity>().Add(attachment);

                message.Body = builder.ToMessageBody() //body //$"{body}"
                    ;

                //message.Body = new TextPart(TextFormat.Html)
                //{
                //    Text = body //$"{body}"
                //};

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("smartattendance45@gmail.com", "Coder@18");

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }


                return;// Task.CompletedTask;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
