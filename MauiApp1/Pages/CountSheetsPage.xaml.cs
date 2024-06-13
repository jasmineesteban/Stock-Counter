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
            new MyData { Name = "Product A", Quantity = 20, UOM = "pcs" },
            new MyData { Name = "Product B", Quantity = 20, UOM = "kg" },
            new MyData { Name = "Product C", Quantity = 20, UOM = "pcs" },
            new MyData { Name = "Product D", Quantity = 20, UOM = "kg" },
            // Add more data items as needed
        };
        BindingContext = this;
    }

    private async void AddItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddItemPage());

    }
}

public class MyData
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string UOM { get; set; } // Unit of Measurement
}