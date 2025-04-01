using DataLayer.Models;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace DocumentsLibrary
{
    public class ExcelService : IDisposable
    {
        private Application excelApp;
        private Workbook workbook;
        private Worksheet worksheet;

        public void CreatReport(string fileName, List<Game> games)
        {
            excelApp = new Application();
            excelApp.Visible = true;
            workbook = excelApp.Workbooks.Add();

            worksheet = (Worksheet)workbook.Worksheets[1];
            worksheet.Name = "Отчет по играм";

            Excel.Range range = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[4][1]];
            range.Merge();
            range.Value = "Отчет по всем играм";
            range.Interior.Color = ColorTranslator.ToOle(Color.BlanchedAlmond);
            range.Borders.LineStyle = XlLineStyle.xlContinuous;
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            worksheet.Cells[2, 1] = "ID";
            worksheet.Cells[2, 2] = "Name";
            worksheet.Cells[2, 3] = "Description";
            worksheet.Cells[2, 4] = "Genre";
            worksheet.Cells[2, 5] = "Cost";
            range = worksheet.Range[worksheet.Cells[1][2], worksheet.Cells[5][2]];
            range.Borders.LineStyle = XlLineStyle.xlContinuous;
            range.Font.Bold = 1;


            for (int row = 0; row < games.Count; row++)
            {
                worksheet.Cells[row + 3, 1] = games[row].Id;
                worksheet.Cells[row + 3, 2] = games[row].Name;
                worksheet.Cells[row + 3, 3] = games[row].Description;
                worksheet.Cells[row + 3, 4] = games[row].Genre.Name;
                worksheet.Cells[row + 3, 5] = 100;

                range = worksheet.Range[worksheet.Cells[1][row + 3], worksheet.Cells[5][row + 3]];
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
            }

            worksheet.Cells[games.Count + 3, 5].Formula = $"=SUM(E3:E{games.Count + 2})";

            worksheet.Columns.AutoFit();

            workbook.SaveAs(fileName);
        }

        public void Dispose()
        {
            if (excelApp != null)
            {
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
            if (worksheet != null)
                Marshal.ReleaseComObject(worksheet);
            if (workbook != null)
                Marshal.ReleaseComObject(workbook);


            worksheet = null;
            workbook = null;
            excelApp = null;

            GC.SuppressFinalize(this);
        }
    }
}
