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
            await page.DisplayAlert("Added", "Count Sheet Added", "OK");
        }

        public static async Task DeleteDataAsync(Page page)
        {
            bool confirmed = await page.DisplayAlert("Confirm Delete", "Are you sure you want to delete?", "Yes", "No");

            if (confirmed)
            {
                await page.DisplayAlert("Delete", "Delete clicked!", "OK");
            }
        }

        public static async Task ExportDataAsync(Page page, ObservableCollection<ItemCount> items)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("CountSheet");

            var headerRange = worksheet.Range("A1:G1");
            headerRange.Style.Font.Italic = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFFF00");
            worksheet.Cell(1, 1).Value = "Ctr";
            worksheet.Cell(1, 2).Value = "Item No.";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "UOM";
            worksheet.Cell(1, 5).Value = "Batch&Lot";
            worksheet.Cell(1, 6).Value = "Expiry";
            worksheet.Cell(1, 7).Value = "Quantity";
            worksheet.Range("A1:G1").SetAutoFilter();

            worksheet.Columns().AdjustToContents();

            int row = 2;
            foreach (var item in items)
            {
                worksheet.Cell(row, 1).Value = item.ItemCounter;
                worksheet.Cell(row, 2).Value = item.ItemCode;
                worksheet.Cell(row, 3).Value = item.ItemDescription;
                worksheet.Cell(row, 4).Value = item.ItemUom;
                worksheet.Cell(row, 5).Value = item.ItemBatchLotNumber;
                worksheet.Cell(row, 6).Value = item.ItemExpiry;
                worksheet.Cell(row, 7).Value = item.ItemQuantity;
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