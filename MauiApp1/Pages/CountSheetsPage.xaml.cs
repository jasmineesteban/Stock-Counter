using System.Collections.ObjectModel;

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
        // EXPORT
        await DisplayAlert("Export", "Export Sample", "OK");
    }

}

public class MyData
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string UOM { get; set; }
}