using System.Collections.ObjectModel;
using MauiApp1.Helpers;
using MauiApp1.Models;
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    public partial class CountSheetsPage : ContentPage
    {
        public ObservableCollection<MyData> Items { get; set; }

        private bool _showName;
        public bool ShowName
        {
            get => _showName;
            set
            {
                _showName = value;
                OnPropertyChanged(nameof(ShowName));
                GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ShowName, ShowQuantity, ShowUOM);
            }
        }

        private bool _showQuantity;
        public bool ShowQuantity
        {
            get => _showQuantity;
            set
            {
                _showQuantity = value;
                OnPropertyChanged(nameof(ShowQuantity));
                GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ShowName, ShowQuantity, ShowUOM);
            }
        }

        private bool _showUOM;
        public bool ShowUOM
        {
            get => _showUOM;
            set
            {
                _showUOM = value;
                OnPropertyChanged(nameof(ShowUOM));
                GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ShowName, ShowQuantity, ShowUOM);
            }
        }

        public CountSheetsPage()
        {
            InitializeComponent();
            Items = new ObservableCollection<MyData>
            {
                new MyData { Name = "Item 1", Quantity = 10, UOM = "kg" },
                new MyData { Name = "Item 2", Quantity = 20, UOM = "ltr" },
                new MyData { Name = "Item 3", Quantity = 30, UOM = "pcs" }
            };

            BindingContext = this;

            // Initial column visibility settings
            ShowName = true;
            ShowQuantity = true;
            ShowUOM = true;

            dataGrid.ItemsSource = Items;
            GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ShowName, ShowQuantity, ShowUOM);
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
                    await DataHelper.SaveDataAsync(this);
                    break;
                case "Delete":
                    await DataHelper.DeleteDataAsync(this);
                    break;
                case "Export":
                    await DataHelper.ExportDataAsync(this, Items);
                    break;
                default:
                    // Handle cancellation or unexpected actions
                    break;
            }
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushModalAsync(new ColumnSelectionPage(this));
        }
    }
}
