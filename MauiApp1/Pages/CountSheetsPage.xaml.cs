using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Collections.ObjectModel;


namespace MauiApp1.Pages
{
    [QueryProperty(nameof(ItemDescription), "ItemDescription")]
    [QueryProperty(nameof(SellingUom), "SellingUom")]
    [QueryProperty(nameof(ItemNumber), "ItemNumber")]
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class CountSheetsPage : ContentPage
    {
        private string? _itemDescription;
        public string? ItemDescription
        {
            get => _itemDescription;
            set => _itemDescription = value;
        }

        private string? _sellingUom;
        public string? SellingUom
        {
            get => _sellingUom;
            set => _sellingUom = value;
        }

        private string? _itemNumber;
        public string? ItemNumber
        {
            get => _itemNumber;
            set => _itemNumber = value;
        }

        private ColumnVisibilityHelper _columnVisibility;
        public ColumnVisibilityHelper ColumnVisibility
        {
            get => _columnVisibility;
            set
            {
                _columnVisibility = value;
                OnPropertyChanged(nameof(ColumnVisibility));
            }
        }

        private void UpdateColumnVisibility()
        {
            GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ColumnVisibility.ShowCtr, ColumnVisibility.ShowItemNo, ColumnVisibility.ShowDescription, ColumnVisibility.ShowUom, ColumnVisibility.ShowBatchLot, ColumnVisibility.ShowExpiry, ColumnVisibility.ShowQuantity, this);
        }

        private string _employeeDetails;
        private string _employeeId;  
        public string EmployeeDetails
        {
            get => _employeeDetails;
            set
            {
                _employeeDetails = value;
                OnPropertyChanged(nameof(EmployeeDetails));

                var details = value.Split(new[] { " - " }, StringSplitOptions.None);
                _employeeId = details[0];
                var employeeName = details.Length > 1 ? details[1] : string.Empty;
                EmployeeDetailsLabel.Text = employeeName;
            }
        }

        public ObservableCollection<ItemCount> ItemCount { get; set; }
        private ItemCountViewModel _itemCountViewModel;
        private readonly string _countCode;
        private Label loadedItemCount;
        private int _sort = 0; 
        private readonly HttpClientService _httpClientService;

        public void ApplyColumnSettings(Dictionary<string, bool> settings)
        {
            ColumnVisibility.ShowItemNo = settings["ShowItemNo"];
            ColumnVisibility.ShowDescription = settings["ShowDescription"];
            ColumnVisibility.ShowUom = settings["ShowUom"];
            ColumnVisibility.ShowBatchLot = settings["ShowBatchLot"];
            ColumnVisibility.ShowExpiry = settings["ShowExpiry"];
            ColumnVisibility.ShowQuantity = settings["ShowQuantity"];

            UpdateColumnVisibility();
        }
        
        public CountSheetsPage(ItemCountViewModel itemCountViewModel, string countCode, int sortValue, HttpClientService httpClientService)
        {
            InitializeComponent();

            // Services and View Models
            _httpClientService = httpClientService;
            _itemCountViewModel = itemCountViewModel;
            _countCode = countCode;

            // Data Collections and Bindings
            ItemCount = new ObservableCollection<ItemCount>();
            dataGrid.ItemsSource = ItemCount;
            BindingContext = this;

            // UI Components
            loadedItemCount = this.FindByName<Label>("LoadedItemCount");

            // Column Visibility Helper
            ColumnVisibility = new ColumnVisibilityHelper();
            ColumnVisibility.PropertyChanged += (sender, e) => UpdateColumnVisibility();
            InitializeVisibilitySettings();
            UpdateColumnVisibility();

            // Initial Data
            LoadItemCountData();

            // tap Gesture for Header Grid
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnHeaderGridTapped;
            HeaderGrid.GestureRecognizers.Add(tapGesture);

            // Sorting
            _sort = sortValue;
        }

        private void InitializeVisibilitySettings()
        {
            ColumnVisibility.ShowCtr = false;
            ColumnVisibility.ShowItemNo = false;
            ColumnVisibility.ShowDescription = true;
            ColumnVisibility.ShowUom = true;
            ColumnVisibility.ShowBatchLot = true;
            ColumnVisibility.ShowExpiry = true;
            ColumnVisibility.ShowQuantity = true;
            UpdateColumnVisibility();
        }

        private async void OnHeaderGridTapped(object sender, EventArgs e)
        {
            await SortingHelper.HandleHeaderGridTap(HeaderGrid, async sortValue =>
            {
                _sort = sortValue;
                LoadItemCountData();
                return await _itemCountViewModel.ShowItemCount(_countCode, _sort);
            });
        }

        private async void LoadItemCountData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;

                var items = await _itemCountViewModel.ShowItemCount(_countCode, _sort);

                ItemCount.Clear();
                foreach (var item in items)
                {
                    ItemCount.Add(item);
                }

                int itemCount = items.Count();
                loadedItemCount.Text = $"Items Counted: {itemCount}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load items: {ex.Message}", "OK");
            }
            finally
            {
                // Hide loading indicator
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
            }
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            var itemViewModel2 = new ItemViewModel2(_httpClientService);
            var itemSelectorPage2 = new ItemSelectorPage2(itemViewModel2);

            itemSelectorPage2.Disappearing += async (s, args) =>
            {
                if (itemSelectorPage2.SelectedItem != null)
                {
                    var result = await AddItemDialogHelper.AddItem(ItemNumber, ItemDescription, SellingUom, _countCode, _itemCountViewModel, _httpClientService);
                    if (result)
                    {
                        LoadItemCountData();
                    }
                }
            };

            await Shell.Current.Navigation.PushAsync(itemSelectorPage2);
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushModalAsync(new ColumnSelectionPage(this));
        }

        private async void ColumnSelection_Clicked(object sender, EventArgs e)
        {
            var columnSelectionPage = new ColumnSelectionPage(this);
            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(columnSelectionPage));
        }

        internal async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is ItemCount selectedItemCount)
            {
                await EditItemDialogHelper.EditItem(selectedItemCount, _itemCountViewModel);
                LoadItemCountData();
            }
        }

        internal async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is ItemCount selectedItemCount)
            {
                bool answer = await DisplayAlert("Delete", $"Are you sure you want to delete {selectedItemCount.ItemDescription}?", "Yes", "No");
                if (answer)
                {
                    try
                    {
                        await _itemCountViewModel.DeleteItemCount(selectedItemCount.ItemKey);
                        var toast = Toast.Make($"Deleted {selectedItemCount.ItemDescription}", ToastDuration.Short);
                        await toast.Show();
                        LoadItemCountData();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete: {ex.Message}", "OK");
                    }
                }
            }
        }
    }
}