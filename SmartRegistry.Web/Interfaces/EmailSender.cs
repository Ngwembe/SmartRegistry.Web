using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
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

                //if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                //{
                //    _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                //}

                //var path = $"{_hostingEnvironment.WebRootPath}\\Reports\\{fileName}";
                
                var builder = new BodyBuilder();

                builder.HtmlBody = "<p>See attached document for the report.";

                //var s = await ReadingAnObject(fileName);
                try
                {
                    GetObjectRequest request = new GetObjectRequest()
                    {
                        BucketName = "elasticbeanstalk-eu-west-2-925426318079",
                        Key = fileName
                    };

                    var client = new AmazonS3Client("AKIAJMKBT2AZBA24LFYA", "DOnaWvOF6RAHLfcaqB3N3q41OmsJeyRxZP+uoNv+", Amazon.RegionEndpoint.USEast2);

                    using (GetObjectResponse response = await client.GetObjectAsync(request))
                    {
                        string title = response.Metadata["x-amz-meta-report"];

                        using (var s = response.ResponseStream)
                        {
                            builder.Attachments.Add(fileName, s);
                            s.Close();
                        }

                        //Console.WriteLine("The object's title is {0}", title);
                        //string dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), keyName);
                        //if (!File.Exists(dest))
                        //{
                        //    await response.WriteResponseStreamToFileAsync(dest, false, new CancellationToken(false));
                        //}
                    }
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null &&
                        (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                         amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        Console.WriteLine("Please check the provided AWS Credentials.");
                        Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                    }
                    else
                    {
                        Console.WriteLine("An error occurred with the message '{0}' when reading an object", amazonS3Exception.Message);
                    }
                }

                //if (s != null)
                //{
                //    builder.Attachments.Add(fileName, s);
                //    s.Close();
                //}
                //builder.Attachments.Add(path);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Smart Attendance", "smartattendance45@gmail.com"));
                //message.From.Add(_systemMailboxAddress);

                message.To.Add(new MailboxAddress(recipientName, recipientEmailAddress));
                message.Subject = subject;

                message.Body = builder.ToMessageBody(); //body //$"{body}"

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    //await client.AuthenticateAsync("tsepo@mgibagroup.com", "Jabulile@009");
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

        async Task<Stream> ReadingAnObject(string keyName)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest()
                {
                    BucketName = "elasticbeanstalk-eu-west-2-925426318079",
                    Key = keyName
                };

                var client = new AmazonS3Client("AKIAJMKBT2AZBA24LFYA", "DOnaWvOF6RAHLfcaqB3N3q41OmsJeyRxZP+uoNv+", Amazon.RegionEndpoint.USEast2);

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                {
                    string title = response.Metadata["x-amz-meta-report"];
                    Console.WriteLine("The object's title is {0}", title);
                    string dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), keyName);
                    if (!File.Exists(dest))
                    {
                        await response.WriteResponseStreamToFileAsync(dest, false, new CancellationToken(false));
                    }

                    return response.ResponseStream;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when reading an object", amazonS3Exception.Message);
                }

                return null;
            }
        }

    }
}
