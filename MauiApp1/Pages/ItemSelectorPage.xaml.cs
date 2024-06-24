using MauiApp1.Services;
using MauiApp1.ViewModels;
using ZXing.Mobile;
using CommunityToolkit;


namespace MauiApp1.Pages
{
    public partial class ItemSelectorPage : ContentPage
    {
        private readonly HttpClientService _httpClientService;

        public ItemSelectorPage(HttpClientService httpClientService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;

            var viewModel = new ItemViewModel(httpClientService);
            BindingContext = viewModel;
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
    }
}
