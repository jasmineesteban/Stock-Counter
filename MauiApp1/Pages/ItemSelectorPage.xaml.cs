using MauiApp1.Services;
using MauiApp1.ViewModels;
using ZXing.Mobile;
using MauiApp1.Models;
using CommunityToolkit;


namespace MauiApp1.Pages
{
    public partial class ItemSelectorPage : ContentPage
    {
        private readonly HttpClientService _httpClientService;

        public AddItemPage AddItemPage { get; }

        public ItemSelectorPage(HttpClientService httpClientService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;

            var viewModel = new ItemViewModel(httpClientService);
            BindingContext = viewModel;
        }

        public ItemSelectorPage(AddItemPage addItemPage)
        {
            AddItemPage = addItemPage;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as ItemViewModel;
            if (viewModel != null && !string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                viewModel.OnSearchBarTextChangedCommand.Execute(e.NewTextValue);
            }
        }

        private async void ScanBarcode_Clicked(object sender, EventArgs e)
        {
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = true,
                UseFrontCameraIfAvailable = false,
            };


            var scanner = new MobileBarcodeScanner
            {
                TopText = "Hold the camera up to the barcode",
                BottomText = "Scanning will happen automatically",
            };

            var result = await scanner.Scan(options);
            if (result != null)
            {
                searchItem.Text = result.Text;
            }
        }

        private async void OnAppearing(object sender, EventArgs e)
        {
            base.OnAppearing();
            await FadeInModalFrame();
        }

        private async Task FadeInModalFrame()
        {
            this.Opacity = 0;
            await this.FadeTo(1, 250, Easing.Linear);
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Item selectedItem)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "ItemDescription", selectedItem.ItemDescription },
                    { "SellingUom", selectedItem.SellingUom }
                };
                await Shell.Current.GoToAsync("..",  parameters);
            }
        }
    }
}
