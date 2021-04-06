using System.Collections.Generic;
using System.IO;
using System.Linq;
using Itamaram.SpecFlow.Plugin.Base;
using OfficeOpenXml;

namespace Itamaram.Excel.SpecFlowPlugin
{
    public class ExcelSource : NamedTagExampleGenerator
    {
        protected override string TagName { get; } = "excel";

        public override IEnumerable<IEnumerable<string>> GetRowsInternal(string args, string dir, string specfile, ExamplesHeader header)
        {
            var parts = args.Split(new[] { ':' }, 2);
            var name = parts[0];
            var worksheet = parts.Length > 1 ? parts[1] : null;

            var columns = header.Count;

            using (var excel = new ExcelPackage(new FileInfo(Path.Combine(dir, name))))
            {
                var sheet = string.IsNullOrEmpty(worksheet)
                    ? excel.Workbook.Worksheets.First()
                    : excel.Workbook.Worksheets[worksheet];

                if(sheet.Dimension == null)
                    return Enumerable.Empty<IEnumerable<string>>();

                return Enumerable.Range(1, sheet.Dimension.End.Row)
                    .Select(i => Enumerable.Range(1, columns).Select(j => sheet.Cells[i, j]))
                    .Select(r => r.Select(c => c.Text ?? string.Empty).ToList())
                    .ToList();
            }
        }
    }
}
