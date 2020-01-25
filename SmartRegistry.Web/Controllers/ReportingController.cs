using System;
using System.Collections.Generic;
using System.IO;
//using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmartRegistry.Web.Interfaces;

namespace SmartRegistry.Web.Controllers
{
    public class ReportingController : Controller
    {
        private readonly IReportingHandler _reportingHandler;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReportingController(IReportingHandler reportingHandler, IEmailSender emailSender, IHostingEnvironment hostingEnvironment)
        {
            _reportingHandler = reportingHandler;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }

        //public async Task<IActionResult> PrintEnrolledStudentList(int id)
        public async Task<IActionResult> PrintEnrolledStudentList(int id)
        {
            try
            {
                var attachment = await _reportingHandler.GetEnrolledSubjectAsync(id);

                if (attachment != null)
                {
                    await _emailSender.SendReportAsync(attachment);
                }

                return RedirectToAction("GetAllEnrolled", "Subjects", new { id = id });

                //var path = $"{_hostingEnvironment.WebRootPath}\\testPDF.pdf";

                //if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                //{
                //    _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                //}

                //var path = $"{_hostingEnvironment.WebRootPath}\\Reports\\{attachment}";

                //FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //MemoryStream ms = new System.IO.MemoryStream();

                //await fs.CopyToAsync(ms);
                ////MemoryStream ms = new System.IO.MemoryStream();

                //byte[] byteInfo = ms.ToArray();
                //ms.Write(byteInfo, 0, byteInfo.Length);
                //ms.Position = 0;

                ////return new FileStreamResult(fs, "application/pdf");
                ////return new FileStreamResult(ms, "application/pdf");
                //return File(new MemoryStream(byteInfo), "application/pdf");

                //using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                //{
                //    MemoryStream ms = new MemoryStream();
                //    await fs.CopyToAsync(ms);


                //    //MemoryStream ms = new System.IO.MemoryStream();

                //    byte[] byteInfo = ms.ToArray();
                //    ms.Write(byteInfo, 0, byteInfo.Length);
                //    ms.Position = 0;

                //    //return new FileStreamResult(fs, "application/pdf");
                //    return new FileStreamResult(ms, "application/pdf");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }




            //return RedirectToAction("GetAllEnrolled", "Subjects", new { id = id });

            //return File(path, "application/pdf");
        }

        public async Task<IActionResult> PrintAttendedStudentList(int id)
        {
            var attachment = await _reportingHandler.GetAttendedStudents(id);

            if (attachment != null)
            {
                await _emailSender.SendReportAsync(attachment);
            }

            //if (!string.IsNullOrWhiteSpace(attachment))
            //{
            //    await _emailSender.SendReportAsync(attachment);
            //}

            if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
            {
                _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var path = $"{_hostingEnvironment.WebRootPath}\\Reports\\{attachment}";
            

            using (FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                MemoryStream ms = new System.IO.MemoryStream();
                await fs.CopyToAsync(ms);


                //MemoryStream ms = new System.IO.MemoryStream();

                byte[] byteInfo = ms.ToArray();
                ms.Write(byteInfo, 0, byteInfo.Length);
                ms.Position = 0;

                //return new FileStreamResult(fs, "application/pdf");
                return new FileStreamResult(ms, "application/pdf");
            }




            //return RedirectToAction("GetAllEnrolled","Subjects", new { id = id });

            //return File(path, "application/pdf");
        }

    }
}