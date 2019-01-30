﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Document = iTextSharp.text.Document;

namespace SmartRegistry.Web.Interfaces
{
    public interface IReportingHandler
    {
        //Task<Document> GetEnrolledSubject(int subjectId);
        //Task<Document> GetSubjectSchedules(int subjectId);

        Task<string> GetEnrolledSubject(int subjectId);
        Task<string> GetSubjectSchedules(int subjectId);

        Task<string> GetAttendedStudents(int subjectId);
    }
}