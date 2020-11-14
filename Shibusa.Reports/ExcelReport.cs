using ClosedXML.Excel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shibusa.Reports
{
    //TODO: Work in progress. Plan is to wrap ClosedXml and simplify a few patterns.
    internal class ExcelReport : FileReport
    {
        private readonly IXLWorkbook workbook;

        public override async Task SaveAsync(Stream stream)
        {
            if (stream?.CanWrite ?? false)
            {
                workbook.SaveAs(stream);
            }
            await Task.CompletedTask;
        }

        public override async Task SaveAsync(string filename)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                if (File.Exists(filename) && !Configuration.OverwriteOnSave)
                {
                    throw new ArgumentException($"The file '{filename}' already exists.");
                }
                workbook.SaveAs(filename);
            }
            await Task.CompletedTask;
        }
    }
}
