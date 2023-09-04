using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Collections.Generic;

namespace HelpCheck_API.Constants
{
    public class ExportToExcel
    {
        public static byte[] Export<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }
    }
}
