using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using SmartRegistry.Web.ViewModels.MailsViewModel;

namespace SmartRegistry.Web.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public async Task<ActionResult> Index()
        {
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Smart Attendance","smartattendance45@gmail.com"));
            //message.To.Add(new MailboxAddress("Tsepo","tsepo.motswiane@gmail.com"));
            //message.To.Add(new MailboxAddress("Tsepo", "tsepo.motswaine@gmail.com"));
            //message.Subject = "Testing Mail Sender from ASP.NET Core 2.0";
            //message.Body = new TextPart(TextFormat.Plain)
            //{
            //    Text = "Hey Tsepo, this is a test from smart attendance"
            //};

            //using (var client = new SmtpClient())
            //{
            //    await client.ConnectAsync("smtp.gmail.com", 587, false);
            //    await client.AuthenticateAsync("smartattendance45@gmail.com","Coder@18");

            //    await client.SendAsync(message);
            //    await client.DisconnectAsync(true);
            //}

            return View();
        }

        // GET: Mail/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mail/Create
        public ActionResult Create()
        {
            return View(new MailSenderViewModel(){ SenderName = "Smart Attendance", SenderEmailAddress = "smartattendance45@gmail.com" });
        }

        // POST: Mail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("SenderName,SenderEmailAddress,RecipientName,RecipientEmailAddress,Subject,Bcc,Content")] MailSenderViewModel email)
        {
            try
            {
                var institution = ViewData["Institution"] ?? null;

                //var message = new MimeMessage();
                //message.From.Add(new MailboxAddress("Smart Attendance", "smartattendance45@gmail.com"));
                //message.To.Add(new MailboxAddress("Tsepo", "tsepo.motswiane@gmail.com"));
                //message.To.Add(new MailboxAddress("Tsepo", "tsepo.motswaine@gmail.com"));
                //message.Subject = "Testing Mail Sender from ASP.NET Core 2.0";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(email.SenderName, email.SenderEmailAddress));
                message.To.Add(new MailboxAddress(email.RecipientName, email.RecipientEmailAddress));
                message.Subject = email.Subject;
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"Hi {email.RecipientName}, <br/><br/>{email.Content}<br/><br/>{institution}"
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("smartattendance45@gmail.com", "Coder@18");

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Mail/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Mail/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mail/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}