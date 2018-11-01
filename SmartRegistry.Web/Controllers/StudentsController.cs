using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Models;

namespace SmartRegistry.Web.Controllers
{
    //[Authorize]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _signInManager = signInManager;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.GetUserAsync(User);
            //var student = _context.Student.FirstOrDefault(s => s.AccountId == user.Id);
            //var userName = User.Identity.Name;
            //var isAdmin = User.IsInRole("Admin");

            //ClaimsPrincipal currentUser = this.User;
            //var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);

            //var user = _userManager.GetUserId(HttpContext.User);

            //var student = _context.Student.FirstOrDefault(s => s.AccountId == user); 
            var students = await _context.Student.ToListAsync();

            //ExportToPDF(students);

            return View(students);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SensorId,AccountId,FirstName,LastName,Age,DOB,IsComplete,Gender,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SensorId,AccountId,FirstName,LastName,Age,DOB,IsComplete,Gender,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,IsDeleted,DeletedBy,DeletedAt")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Student.Remove(student);
            student.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> EnrollStudent(int id /*id*/)
        {   
            
            var subject = await _context.Subject.FirstOrDefaultAsync(s => s.Id == id);
            if (subject == null) return NotFound(); //RedirectToAction("Details", "Subject", new { id = id });

            var user = await _userManager.GetUserAsync(this.User);

            if (user == null) return NotFound(); //return RedirectToAction("Details", "Subject", new { id = id});

            var userId = await _userManager.GetUserIdAsync(user);

            if (userId == null) return NotFound(); //return RedirectToAction("Details", "Subject", new { id = id });

            var student = await _context.Student.FirstOrDefaultAsync(l => l.AccountId == userId);

            if (student == null) RedirectToAction("Details", "Subjects", new { id = id });

            var enrolled = new EnrolledSubjects()
            {
                StudentId = student.Id,
               SubjectId = id
            };

            _context.EnrolledSubject.Add(enrolled);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Subjects", new { id = id });
        }

        //  Reporting 
        private async void ExportToPDF(List<Student> students)
        {
            try
            {
                var pdfDoc = new iTextSharp.text.Document(PageSize.LETTER, 40f, 40f, 60f, 60f);
                //string path = Path.GetPathRoot(@"~\PDFGenerator\PDFGenerator\wwwroot\documents\test.pdf");

                var path = $"{_hostingEnvironment.WebRootPath}\\testPDF.pdf";

                PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.OpenOrCreate)); //c:\users\tsepo\source\repos\PDFGenerator\PDFGenerator\wwwroot\favicon.ico
                pdfDoc.Open();

                //var logoPath = @"c:\users\tsepo\source\repos\PDFGenerator\PDFGenerator\wwwroot\images\logo.png";
                //using (FileStream fs = new FileStream())
                //{
                //    var png = Image.GetInstance(Image.FromStream(fs)), ImageFormat.Png);
                //}

                var spacer = new Paragraph("")
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 10f
                };

                pdfDoc.Add(spacer);

                var headerTable = new PdfPTable(new[] { .75f, 2f })
                {
                    HorizontalAlignment = 25,//TabStop.Alignment.LEFT,
                    WidthPercentage = 75,
                    DefaultCell = { MinimumHeight = 22f }
                };

                headerTable.AddCell("Date");
                headerTable.AddCell(DateTime.Now.ToString());
                headerTable.AddCell("Name");
                headerTable.AddCell("Tsepo");
                headerTable.AddCell("Student Number");
                headerTable.AddCell("213500195");
                headerTable.AddCell("Subject Name");
                headerTable.AddCell("IDC30BT");

                pdfDoc.Add(headerTable);
                pdfDoc.Add(spacer);

                if (!students.Any())
                {
                    pdfDoc.Close();
                    return;                    
                }

                var columnCount = 4;
                var columnWidth = new[] { 0.75f, 1f, 0.75f, 2f };

                var table = new PdfPTable(columnWidth)
                {
                    HorizontalAlignment = 25,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f }
                };

                var cell = new PdfPCell(new Phrase("Student Summary for IDC30BT (Second Semester)", new Font(Font.FontFamily.HELVETICA, 15f)))
                {
                    Colspan = 4,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    MinimumHeight = 30f
                };


                table.AddCell(cell);

                table.AddCell("First Name");
                table.AddCell("Last Name");
                table.AddCell("Gender");
                table.AddCell("Date Of Birth");



                students.ToList().ForEach(s =>
                {
                    table.AddCell(s.FirstName);
                    table.AddCell(s.LastName);
                    table.AddCell(s.Gender);
                    table.AddCell(s.DOB.ToString());
                });

                pdfDoc.Add(table);



                pdfDoc.Close();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
