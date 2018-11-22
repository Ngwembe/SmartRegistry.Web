using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SmartRegistry.Web.Data;
using SmartRegistry.Web.Interfaces;
using SmartRegistry.Web.ViewModels.SubjectViewModels;

namespace SmartRegistry.Web.Domain
{
    public class ReportingHandler : IReportingHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReportingHandler(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<Document> GetEnrolledSubject(int subjectId)
        {
            try
            {
                var subject = await _context.Subject
                    .Include(s => s.Course)
                    .Include(s => s.Lecturer)
                    .SingleOrDefaultAsync(m => m.Id == subjectId);

                if (subject == null) return null;

                var students = await _context.EnrolledSubject
                    .Where(s => s.SubjectId == subjectId)
                    .Include(s => s.Student)
                    .Include(s => s.Subject)
                    //.ThenInclude(s => s.EnrolledSubject)
                    //.Where(s=> s.SubjectId == id)
                    .Select(en => new EnrolledStudentViewModel
                    {
                        SubjectId = subjectId,

                        StudentId = en.StudentId,
                        FirstName = en.Student.FirstName,
                        LastName = en.Student.LastName,
                        StudentNumber = en.Student.StudentNumber,
                        Gender = en.Student.Gender,
                        DOB = en.Student.DOB,

                        SubjectCode = en.Subject.Code,
                        SubjectName = en.Subject.Name
                    }).ToListAsync();

                if (!students.Any())
                {
                    return null;
                }

                var pdfDoc = new iTextSharp.text.Document(PageSize.LETTER, 40f, 40f, 60f, 60f);

                var path = $"{_hostingEnvironment.WebRootPath}";

                PdfWriter.GetInstance(pdfDoc, new FileStream($"{path}\\testPDF.pdf", FileMode.OpenOrCreate));
                pdfDoc.Open();
                
                iTextSharp.text.Image image = Image.GetInstance($"{path}\\images\\logo.png");
                image.ScalePercent(24f);
                //pdfDoc.Add(image);

                var spacer = new Paragraph("")
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 10f
                };

                pdfDoc.Add(spacer);

                //var headerTable = new PdfPTable(new[] { .75f, 2f })
                //{
                //    HorizontalAlignment = 25,//TabStop.Alignment.LEFT,
                //    WidthPercentage = 75,
                //    DefaultCell = { MinimumHeight = 22f }
                //};

                var headerTable = new PdfPTable(new[] { .75f, 2.25f })
                {
                    /*HorizontalAlignment = 25,*/ //TabStop.Alignment.LEFT,
                    //WidthPercentage = 75,
                    //HorizontalAlignment = 5,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f }
                };

                headerTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                headerTable.AddCell("Date");
                headerTable.AddCell(DateTime.Now.ToString("R"));
                headerTable.AddCell("Lecturer Name");
                headerTable.AddCell($"{subject.Lecturer.FirstName} {subject.Lecturer.LastName}");
                headerTable.AddCell("Subject Name");
                headerTable.AddCell($"{subject.Name}");
                headerTable.AddCell("Subject Code");
                headerTable.AddCell($"{subject.Code}");

                image.ScaleToFit(250f, 250f);
                //image.Alignment = Image.TEXTWRAP | Image.ALIGN_RIGHT;
                image.Alignment = Image.TEXTWRAP | Image.ALIGN_LEFT;
                image.IndentationLeft = 9f;
                image.SpacingAfter = 9f;
                image.BorderWidthTop = 36f;
                image.BorderColorTop = BaseColor.WHITE;

                pdfDoc.Add(image);
                pdfDoc.Add(headerTable);
                pdfDoc.Add(spacer);



                //var columnCount = 4;
                //var columnWidth = new[] { 0.75f, 1f, 0.75f, 2f };
                var columnWidth = new[] { 2f, 2f, 0.75f, 2f };

                var table = new PdfPTable(columnWidth)
                {
                    HorizontalAlignment = 25,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f }
                };

                var cell = new PdfPCell(new Phrase($"Student Summary for {subject.Name} ({subject.Code}) (Second Semester)", new Font(Font.FontFamily.HELVETICA, 15f)))
                {
                    Colspan = 4,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    MinimumHeight = 30f


                    ,BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204)
                };


                table.AddCell(cell);

                table.AddCell(new PdfPCell(new Phrase("First Name")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });
                table.AddCell(new PdfPCell(new Phrase("Last Name")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });
                table.AddCell(new PdfPCell(new Phrase("Gender")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });
                table.AddCell(new PdfPCell(new Phrase("Date of Birth")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });



                students.ToList().ForEach(s =>
                {
                    table.AddCell(s.FirstName);
                    table.AddCell(s.LastName);
                    table.AddCell(s.Gender);
                    table.AddCell(s.DOB.ToString());
                });

                cell = new PdfPCell(new Phrase($"Total: {students.Count().ToString()}" /*, new Font(Font.FontFamily.HELVETICA, 15f)*/))
                {
                    //Colspan = 4,
                    //HorizontalAlignment = Element.ALIGN_RIGHT,
                    //MinimumHeight = 30f

                    Colspan = 4,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    MinimumHeight = 30f
                };

                table.AddCell(cell);
                pdfDoc.Add(table);

                //var footerTable = new PdfPTable(new[] { .75f, 2.25f })
                //{
                //    /*HorizontalAlignment = 25,*/ //TabStop.Alignment.LEFT,
                //    //WidthPercentage = 75,
                //    //HorizontalAlignment = 5,
                //    WidthPercentage = 100,
                //    DefaultCell = { MinimumHeight = 22f }
                //};

                //var spacerTableCell = new PdfPTable(new[] {.75f, 2.25f})
                //{
                //    /*HorizontalAlignment = 25,*/ //TabStop.Alignment.LEFT,
                //    WidthPercentage = 75,
                //    HorizontalAlignment = 5,         
                //    DefaultCell = { MinimumHeight = 22f }
                //};

                //footerTable.HorizontalAlignment = Element.ALIGN_RIGHT; //Element.ALIGN_JUSTIFIED;
                //pdfDoc.Add(spacerTableCell);
                //footerTable.AddCell("Total Number");
                //footerTable.AddCell(students.Count().ToString());

                //pdfDoc.Add(footerTable);


                pdfDoc.Close();
                return pdfDoc;

            }
            catch (Exception ex)
            {
                throw;
            }

            //try
            //{
            //    var subject = await _context.Subject
            //        .Include(s => s.Course)
            //        .Include(s => s.Lecturer)
            //        .SingleOrDefaultAsync(m => m.Id == subjectId);

            //    if (subject == null) return null;

            //    var students = await _context.EnrolledSubject
            //        .Where(s => s.SubjectId == subjectId)
            //        .Include(s => s.Student)
            //        .Include(s => s.Subject)
            //        //.ThenInclude(s => s.EnrolledSubject)
            //        //.Where(s=> s.SubjectId == id)
            //        .Select(en => new EnrolledStudentViewModel
            //        {
            //            SubjectId = subjectId,

            //            StudentId = en.StudentId,
            //            FirstName = en.Student.FirstName,
            //            LastName = en.Student.LastName,
            //            StudentNumber = en.Student.StudentNumber,
            //            Gender = en.Student.Gender,
            //            DOB = en.Student.DOB,

            //            SubjectCode = en.Subject.Code,
            //            SubjectName = en.Subject.Name
            //        }).ToListAsync();

            //    if (!students.Any())
            //    {
            //        return null;
            //    }

            //    var pdfDoc = new iTextSharp.text.Document(PageSize.LETTER, 40f, 40f, 60f, 60f);
            //    //string path = Path.GetPathRoot(@"~\PDFGenerator\PDFGenerator\wwwroot\documents\test.pdf");

            //    var path = $"{_hostingEnvironment.WebRootPath}";

            //    PdfWriter.GetInstance(pdfDoc, new FileStream($"{path}\\testPDF.pdf", FileMode.OpenOrCreate)); //c:\users\tsepo\source\repos\PDFGenerator\PDFGenerator\wwwroot\favicon.ico
            //    pdfDoc.Open();

            //    //var logoPath = @"c:\users\tsepo\source\repos\PDFGenerator\PDFGenerator\wwwroot\images\logo.png";
            //    //using (FileStream fs = new FileStream())
            //    //{
            //    //    var png = Image.GetInstance(Image.FromStream(fs)), ImageFormat.Png);
            //    //}

            //    iTextSharp.text.Image image = Image.GetInstance($"{path}\\images\\logo.png");
            //    image.ScalePercent(24f);
            //    pdfDoc.Add(image);

            //    var spacer = new Paragraph("")
            //    {
            //        SpacingBefore = 10f,
            //        SpacingAfter = 10f
            //    };

            //    pdfDoc.Add(spacer);

            //    var headerTable = new PdfPTable(new[] { .75f, 2f })
            //    {
            //        HorizontalAlignment = 25,//TabStop.Alignment.LEFT,
            //        WidthPercentage = 75,
            //        DefaultCell = { MinimumHeight = 22f }
            //    };

            //    headerTable.AddCell("Date");
            //    headerTable.AddCell(DateTime.Now.ToString("R"));
            //    headerTable.AddCell("Lecturer Name");
            //    headerTable.AddCell($"{subject.Lecturer.FirstName} {subject.Lecturer.LastName}");
            //    headerTable.AddCell("Subject Name");
            //    headerTable.AddCell($"{subject.Name}");
            //    headerTable.AddCell("Subject Code");
            //    headerTable.AddCell($"{subject.Code}");

            //    pdfDoc.Add(headerTable);
            //    pdfDoc.Add(spacer);



            //    //var columnCount = 4;
            //    var columnWidth = new[] { 0.75f, 1f, 0.75f, 2f };

            //    var table = new PdfPTable(columnWidth)
            //    {
            //        HorizontalAlignment = 25,
            //        WidthPercentage = 100,
            //        DefaultCell = { MinimumHeight = 22f }
            //    };

            //    var cell = new PdfPCell(new Phrase($"Student Summary for {subject.Name} ({subject.Code}) (Second Semester)", new Font(Font.FontFamily.HELVETICA, 15f)))
            //    {
            //        Colspan = 4,
            //        HorizontalAlignment = Element.ALIGN_CENTER,
            //        MinimumHeight = 30f
            //    };


            //    table.AddCell(cell);

            //    table.AddCell("First Name");
            //    table.AddCell("Last Name");
            //    table.AddCell("Gender");
            //    table.AddCell("Date Of Birth");



            //    students.ToList().ForEach(s =>
            //    {
            //        table.AddCell(s.FirstName);
            //        table.AddCell(s.LastName);
            //        table.AddCell(s.Gender);
            //        table.AddCell(s.DOB.ToString());
            //    });

            //    pdfDoc.Add(table);



            //    pdfDoc.Close();

            //    return pdfDoc;

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        public async Task<Document> GetSubjectSchedules(int subjectId)
        {
            try
            {
                var subject = await _context.Subject
                    .Include(s => s.Course)
                    .Include(s => s.Lecturer)
                    .SingleOrDefaultAsync(m => m.Id == subjectId);

                if (subject == null) return null;

                var schedules = await _context.Schedule
                    .Where(s => s.SubjectId == subjectId)
                    .Include(s => s.Subject)
                    .ToListAsync();

                if (!schedules.Any())
                {
                    return null;
                }

                var pdfDoc = new iTextSharp.text.Document(PageSize.LETTER, 40f, 40f, 60f, 60f);

                var path = $"{_hostingEnvironment.WebRootPath}";

                PdfWriter.GetInstance(pdfDoc, new FileStream($"{path}\\testPDF.pdf", FileMode.OpenOrCreate));
                pdfDoc.Open();

                iTextSharp.text.Image image = Image.GetInstance($"{path}\\images\\logo.png");
                image.ScalePercent(24f);

                var spacer = new Paragraph("")
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 10f
                };

                pdfDoc.Add(spacer);

                var headerTable = new PdfPTable(new[] { .75f, 2.25f })
                {
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f }
                };

                headerTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                headerTable.AddCell("Date");
                headerTable.AddCell(DateTime.Now.ToString("R"));
                headerTable.AddCell("Lecturer Name");
                headerTable.AddCell($"{subject.Lecturer.FirstName} {subject.Lecturer.LastName}");
                headerTable.AddCell("Subject Name");
                headerTable.AddCell($"{subject.Name}");
                headerTable.AddCell("Subject Code");
                headerTable.AddCell($"{subject.Code}");

                image.ScaleToFit(250f, 250f);
                image.Alignment = Image.TEXTWRAP | Image.ALIGN_LEFT;
                image.IndentationLeft = 9f;
                image.SpacingAfter = 9f;
                image.BorderWidthTop = 36f;
                image.BorderColorTop = BaseColor.WHITE;

                pdfDoc.Add(image);
                pdfDoc.Add(headerTable);
                pdfDoc.Add(spacer);

                var columnWidth = new[] { 2f, 2f, 0.75f, 2f };

                var table = new PdfPTable(columnWidth)
                {
                    HorizontalAlignment = 25,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f }
                };

                var cell = new PdfPCell(new Phrase($"Schedules Summary for {subject.Name} ({subject.Code}) (Second Semester)", new Font(Font.FontFamily.HELVETICA, 15f)))
                {
                    Colspan = 4,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    MinimumHeight = 30f


                    ,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204)
                };


                table.AddCell(cell);

                table.AddCell(new PdfPCell(new Phrase("Start Time")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });
                table.AddCell(new PdfPCell(new Phrase("End Time")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });
                table.AddCell(new PdfPCell(new Phrase("Lecture Room")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });
                table.AddCell(new PdfPCell(new Phrase("Schedule Confirmed")) { MinimumHeight = 30f, BackgroundColor = new iTextSharp.text.BaseColor(255, 204, 204) });



                schedules.ToList().ForEach(s =>
                {
                    table.AddCell(s.ScheduleFor.ToString("f"));
                    table.AddCell(s.ScheduleTo.ToString("f"));
                    table.AddCell(s.LectureRoom);
                    table.AddCell(s.IsConfirmed ? "Confirmed" : "Not Confirmed");
                });

                cell = new PdfPCell(new Phrase($"Total: {schedules.Count().ToString()}" /*, new Font(Font.FontFamily.HELVETICA, 15f)*/))
                {
                    Colspan = 4,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    MinimumHeight = 30f
                };

                table.AddCell(cell);
                pdfDoc.Add(table);
                
                pdfDoc.Close();
                return pdfDoc;

            }
            catch (Exception ex)
            {
                throw;
            }

            
        }
    }
}
