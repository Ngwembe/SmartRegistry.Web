using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Document = iTextSharp.text.Document;

namespace SmartRegistry.Web.Interfaces
{
    public interface IReportingHandler
    {
        Task<Document> GetEnrolledSubject(int subjectId);
    }
}
