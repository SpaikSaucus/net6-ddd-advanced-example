using AuthorizationOperation.Domain.Authorization.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AuthorizationOperation.Infrastructure.Services.ReportFile
{
    public class ReportFileExcel : IReportFile
    {
        public MemoryStream Create(IEnumerable<Authorization> authorizationOperations)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, @"Resources\report_auth_op.xlsx");

            using var workbook = new XLWorkbook(filePath);
            var stream = new MemoryStream();
            var sampleSheet = workbook.Worksheets.Where(x => x.Name == "Report1").First();

            //Add your logic here!
            //Example
            sampleSheet.Cell("C3").Value = DateTime.Now.ToString();
            sampleSheet.Cell("C4").Value = authorizationOperations.Count();

            workbook.SaveAs(stream);

            return stream;
        }
    }
}
