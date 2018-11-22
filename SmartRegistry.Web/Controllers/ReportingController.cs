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
            var attachment = await _reportingHandler.GetEnrolledSubject(id);

            if (attachment != null)
            {
                await _emailSender.SendReportAsync();
            }

            var path = $"{_hostingEnvironment.WebRootPath}\\testPDF.pdf";

            FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            MemoryStream ms = new System.IO.MemoryStream();
            await fs.CopyToAsync(ms);


            //MemoryStream ms = new System.IO.MemoryStream();

            byte[] byteInfo = ms.ToArray();
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;

            return new FileStreamResult(fs, "application/pdf");
            //return new FileStreamResult(ms, "application/pdf");

            //return File(fs, "application/pdf");

            //return RedirectToAction("GetAllEnrolled","Subjects", new { id = id });

            //return File(path, "application/pdf");
        }
    }
}