using System.Collections.ObjectModel;
using MauiApp1.Helpers;
using MauiApp1.Models;
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    public partial class CountSheetsPage : ContentPage
    {

        public ObservableCollection<ItemCount> Items { get; set; }

        private bool _showCtr;
        public bool ShowCtr
        {
            get => _showCtr;
            set
            {
                _showCtr = value;
                OnPropertyChanged(nameof(ShowCtr));
                UpdateColumnVisibility();
            }
        }

        private bool _showItemNo;
        public bool ShowItemNo
        {
            get => _showItemNo;
            set
            {
                _showItemNo = value;
                OnPropertyChanged(nameof(ShowItemNo));
                UpdateColumnVisibility();
            }
        }

        private bool _showDescription;
        public bool ShowDescription
        {
            get => _showDescription;
            set
            {
                _showDescription = value;
                OnPropertyChanged(nameof(ShowDescription));
                UpdateColumnVisibility();
            }
        }

        private bool _showUom;
        public bool ShowUom
        {
            get => _showUom;
            set
            {
                _showUom = value;
                OnPropertyChanged(nameof(ShowUom));
                UpdateColumnVisibility();
            }
        }

        private bool _showBatchLot;
        public bool ShowBatchLot
        {
            get => _showBatchLot;
            set
            {
                _showBatchLot = value;
                OnPropertyChanged(nameof(ShowBatchLot));
                UpdateColumnVisibility();
            }
        }

        private bool _showExpiry;
        public bool ShowExpiry
        {
            get => _showExpiry;
            set
            {
                _showExpiry = value;
                OnPropertyChanged(nameof(ShowExpiry));
                UpdateColumnVisibility();
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
                UpdateColumnVisibility();
            }
        }

        private void UpdateColumnVisibility()
        {
            GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ShowCtr, ShowItemNo, ShowDescription, ShowUom, ShowBatchLot, ShowExpiry, ShowQuantity);
        }

        public CountSheetsPage()
        {
            InitializeComponent();
            Items = new ObservableCollection<ItemCount>
            {
            };
            BindingContext = this;
            // Initial column visibility settings
            ShowCtr = true;
            ShowItemNo = true;
            ShowDescription = true;
            ShowUom = true;
            ShowBatchLot = true;
            ShowExpiry = true;
            ShowQuantity = true;
            dataGrid.ItemsSource = Items;
            UpdateColumnVisibility();
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

        private void FilteredSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = e.NewTextValue?.ToLower() ?? string.Empty;
            dataGrid.ItemsSource = Items.Where(item => item.ItemDescription.ToLower().Contains(filterText)).ToList();
        }

        private  async void Filter_Clicked(object sender, EventArgs e)
        {
            // Implement your filter logic here
        await    Shell.Current.Navigation.PushModalAsync(new ColumnSelectionPage(this));
        }

        private async void ColumnSelection_Clicked(object sender, EventArgs e)
        {
            var columnSelectionPage = new ColumnSelectionPage(this);
            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(columnSelectionPage));
        }
    }
}
