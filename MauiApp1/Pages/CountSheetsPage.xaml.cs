using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;

namespace MauiApp1.Pages;

public partial class CountSheetsPage : ContentPage
{
    public ObservableCollection<MyData> Items { get; set; }

    public CountSheetsPage()
    {
        InitializeComponent();
        Items = new ObservableCollection<MyData>
        {
            // SAMPLE
            new MyData { Name = "Product A", Quantity = 20, UOM = "pcs" },
            new MyData { Name = "Product B", Quantity = 20, UOM = "kg" },
            new MyData { Name = "Product C", Quantity = 20, UOM = "pcs" },
            new MyData { Name = "Product D", Quantity = 20, UOM = "kg" },
        };
        BindingContext = this;
    }

    private async void AddItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddItemPage());

    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Action", "Cancel", null, "Save", "Delete", "Export");

        switch (action)
        {
            case "Save":
                await SaveDataAsync();
                break;
            case "Delete":
                await DeleteDataAsync();
                break;
            case "Export":
                await ExportDataAsync();
                break;
            default:
                // Handle cancellation or unexpected actions
                break;
        }
    }

    private async Task SaveDataAsync()
    {
        // SAVE
        await DisplayAlert("Added", "Count Sheet Added", "OK");
    }

    private async Task DeleteDataAsync()
    {
        bool confirmed = await DisplayAlert("Confirm Delete", "Are you sure you want to delete?", "Yes", "No");

        if (confirmed)
        {
            // DELETE
            await DisplayAlert("Delete", "Delete clicked!", "OK");
        }
        else
        {
            // Handle cancellation 
        }
    }
    private async Task ExportDataAsync()
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
        foreach (var item in Items)
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
                await DisplayAlert("Export Successful", $"Your file '{fileName}' has been exported to: {filePath}", "OK");
            }
            else
            {
                await DisplayAlert("Export Cancelled", "The export process was cancelled.", "OK");
            }
        }
    }


    public class MyData
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public string? UOM { get; set; }
    }
}