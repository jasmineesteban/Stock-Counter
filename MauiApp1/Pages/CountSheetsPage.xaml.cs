using System.Collections.ObjectModel;
using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.ViewModels;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class CountSheetsPage : ContentPage
    {


        //private bool _showCtr;
        //public bool ShowCtr
        //{
        //    get => _showCtr;
        //    set
        //    {
        //        _showCtr = value;
        //        OnPropertyChanged(nameof(ShowCtr));
        //        UpdateColumnVisibility();
        //    }
        //}

        //private bool _showItemNo;
        //public bool ShowItemNo
        //{
        //    get => _showItemNo;
        //    set
        //    {
        //        _showItemNo = value;
        //        OnPropertyChanged(nameof(ShowItemNo));
        //        UpdateColumnVisibility();
        //    }
        //}

        //private bool _showDescription;
        //public bool ShowDescription
        //{
        //    get => _showDescription;
        //    set
        //    {
        //        _showDescription = value;
        //        OnPropertyChanged(nameof(ShowDescription));
        //        UpdateColumnVisibility();
        //    }
        //}

        //private bool _showUom;
        //public bool ShowUom
        //{
        //    get => _showUom;
        //    set
        //    {
        //        _showUom = value;
        //        OnPropertyChanged(nameof(ShowUom));
        //        UpdateColumnVisibility();
        //    }
        //}

        //private bool _showBatchLot;
        //public bool ShowBatchLot
        //{
        //    get => _showBatchLot;
        //    set
        //    {
        //        _showBatchLot = value;
        //        OnPropertyChanged(nameof(ShowBatchLot));
        //        UpdateColumnVisibility();
        //    }
        //}

        //private bool _showExpiry;
        //public bool ShowExpiry
        //{
        //    get => _showExpiry;
        //    set
        //    {
        //        _showExpiry = value;
        //        OnPropertyChanged(nameof(ShowExpiry));
        //        UpdateColumnVisibility();
        //    }
        //}

        //private bool _showQuantity;
        //public bool ShowQuantity
        //{
        //    get => _showQuantity;
        //    set
        //    {
        //        _showQuantity = value;
        //        OnPropertyChanged(nameof(ShowQuantity));
        //        UpdateColumnVisibility();
        //    }
        //}

        //private void UpdateColumnVisibility()
        //{
        //    GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ShowCtr, ShowItemNo, ShowDescription, ShowUom, ShowBatchLot, ShowExpiry, ShowQuantity);
        //}


        private string _employeeDetails;
        private string _employeeId;  // Private field to store EmployeeId
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
        public CountSheetsPage(ItemCountViewModel itemCountViewModel, string countCode)
        {
            InitializeComponent();
            _itemCountViewModel = itemCountViewModel;
            ItemCount = new ObservableCollection<ItemCount>();
            _countCode = countCode;
            BindingContext = this;

            //ShowCtr = true;
            //ShowItemNo = true;
            //ShowDescription = true;
            //ShowUom = true;
            //ShowBatchLot = true;
            //ShowExpiry = true;
            //ShowQuantity = true;
            dataGrid.ItemsSource = ItemCount;
            //UpdateColumnVisibility();

            // Load data when the page is constructed
            LoadItemCountData();
            MessagingCenter.Subscribe<AddItemPage, string>(this, "RefreshItemCount", (sender, code) =>
            {
                if (code == _countCode)
                {
                    LoadItemCountData();
                }
            });
        }


        private async void LoadItemCountData()
        {
            try
            {
                // Assuming you have a way to get the current countCode
                string countCode = _countCode; // Replace with actual logic to get countCode
                var items = await _itemCountViewModel.ShowItemCount(countCode);
                ItemCount.Clear();
                foreach (var item in items)
                {
                    ItemCount.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load items: {ex.Message}", "OK");
            }
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is CountSheet selectedCountSheet)
            {
                string countCode = selectedCountSheet.CountCode;
                var addItemPage = new AddItemPage(countCode, _itemCountViewModel);
                await Navigation.PushAsync(addItemPage);
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await DataHelper.ExportDataAsync(this, ItemCount);
        }

       
        private async void Filter_Clicked(object sender, EventArgs e)
        {
            // Implement your filter logic here
            await Shell.Current.Navigation.PushModalAsync(new ColumnSelectionPage(this));
        }

        private async void ColumnSelection_Clicked(object sender, EventArgs e)
        {
            var columnSelectionPage = new ColumnSelectionPage(this);
            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(columnSelectionPage));
        }
    }
}
