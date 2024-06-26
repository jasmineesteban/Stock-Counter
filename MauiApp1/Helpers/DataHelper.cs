using ClosedXML.Excel;
using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;
using MauiApp1.Models;

namespace MauiApp1.Helpers
{
    public static class DataHelper
    {
        public static async Task SaveDataAsync(Page page)
        {
            // SAVE
            await page.DisplayAlert("Added", "Count Sheet Added", "OK");
        }

        public static async Task DeleteDataAsync(Page page)
        {
            bool confirmed = await page.DisplayAlert("Confirm Delete", "Are you sure you want to delete?", "Yes", "No");

            if (confirmed)
            {
                // DELETE
                await page.DisplayAlert("Delete", "Delete clicked!", "OK");
            }
            else
            {
                // Handle cancellation 
            }
        }

        public static async Task ExportDataAsync(Page page, ObservableCollection<MyData> items)
        {
            // Create a new workbook
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("CountSheet");

            // Add headers with styling
            var headerRange = worksheet.Range("A1:C1");
            headerRange.Style.Font.Italic = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFFF00");
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Quantity";
            worksheet.Cell(1, 3).Value = "UOM";
            worksheet.Range("A1:C1").SetAutoFilter();

            // Auto-fit columns for better readability
            worksheet.Columns().AdjustToContents();

            // Add data rows
            int row = 2;
            foreach (var item in items)
            {
                worksheet.Cell(row, 1).Value = item.Name;
                worksheet.Cell(row, 2).Value = item.Quantity;
                worksheet.Cell(row, 3).Value = item.UOM;
                row++;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Position = 0;
                var fileName = $"CountSheet_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                var result = await FileSaver.Default.SaveAsync(fileName, stream);

                if (result.IsSuccessful)
                {
                    string filePath = result.FilePath;
                    await page.DisplayAlert("Export Successful", $"Your file '{fileName}' has been exported to: {filePath}", "OK");
                }
                else
                {
                    await page.DisplayAlert("Export Cancelled", "The export process was cancelled.", "OK");
                }
            }
        }
    }
}
