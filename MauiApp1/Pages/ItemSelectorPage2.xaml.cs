using MauiApp1.ViewModels;
using MauiApp1.Helpers;
using System.Collections.ObjectModel;
using MauiApp1.Models;

namespace MauiApp1.Pages
{
    public partial class ItemSelectorPage2 : ContentPage
    {
        private readonly ItemViewModel2 _viewModel;
        private readonly ObservableCollection<Item> _items;
        public Item SelectedItem { get; private set; }

        public ItemSelectorPage2(ItemViewModel2 viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _items = new ObservableCollection<Item>();
            ItemsCollectionView.ItemsSource = _items;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadItems();
        }

        private async Task LoadItems(string pattern = "")
        {
            await DataLoader.LoadDataAsync(_items, () => _viewModel.GetItem(pattern), LoadingIndicator);
        }

        private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadItems(e.NewTextValue);
        }

        private async void ScanBarcode_Clicked(object sender, EventArgs e)
        {
            var result = await BarcodeHelper.ScanBarcodeAsync();
            if (!string.IsNullOrEmpty(result))
            {
                ItemSearchBar.Text = result;
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Item selectedItem)
            {
                SelectedItem = selectedItem;
                var parameters = new Dictionary<string, object>
                {
                    { "ItemDescription", selectedItem.ItemDescription },
                    { "SellingUom", selectedItem.SellingUom },
                    { "ItemNumber", selectedItem.ItemNumber }
                };
                await Shell.Current.GoToAsync("..", parameters);
            }
            else
            {
                SelectedItem = null;
            }

            await FadeOutPage();
        }

        private async Task FadeOutPage()
        {
            await this.FadeTo(0, 500, Easing.CubicInOut);
        }
    }
}
