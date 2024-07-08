using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Collections.ObjectModel;
using MauiApp1.Models;
using ZXing.Mobile;

namespace MauiApp1.Pages;
public partial class ItemSelectorPage2 : ContentPage
{
    private readonly ItemViewModel2 _viewModel;
    private ObservableCollection<Item> _items;
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
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        try
        {
            var items = await _viewModel.GetItem(pattern);
            _items.Clear();
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load items: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        await LoadItems(e.NewTextValue);
    }

    private async void ScanBarcode_Clicked(object sender, EventArgs e)
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }

        if (status == PermissionStatus.Granted)
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
                ItemSearchBar.Text = result.Text;
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Permission Denied", "Camera permission is required to scan barcodes.", "OK");
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
    }
}

