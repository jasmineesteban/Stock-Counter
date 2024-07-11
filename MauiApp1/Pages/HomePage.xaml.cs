using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Collections.ObjectModel;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class HomePage : ContentPage
    {
        private readonly CountSheetViewModel _countSheetViewModel;
        private readonly ItemCountService _itemCountService;
        private readonly HttpClientService _httpClientService;
        private readonly EditCountDialogHelper _editCountDialogHelper;
        private readonly HomeHelper _homeHelper;
        private bool isNavigating = false;

        private string employeeDetails;
        private string employeeId;
        private string employeeName;

        public string EmployeeDetails
        {
            get => employeeDetails;
            set
            {
                employeeDetails = value;
                var details = value.Split(new[] { " - " }, StringSplitOptions.None);
                employeeId = details[0];
                employeeName = details.Length > 1 ? details[1] : string.Empty;
                OnPropertyChanged(nameof(EmployeeName));
                LoadCountSheets();
            }
        }

        public string EmployeeName => employeeName;
        public ObservableCollection<CountSheet> CountSheets { get; set; } = new ObservableCollection<CountSheet>();

        public HomePage(CountSheetViewModel countSheetViewModel, ItemCountService itemCountService, HttpClientService httpClientService)
        {
            InitializeComponent();
            _countSheetViewModel = countSheetViewModel;
            _itemCountService = itemCountService;
            _httpClientService = httpClientService;
            _editCountDialogHelper = new EditCountDialogHelper(_countSheetViewModel);
            _homeHelper = new HomeHelper(_countSheetViewModel);

            BindingContext = this;
        }

        private async void LoadCountSheets()
        {
            await DataLoader.LoadDataAsync(CountSheets, () => _countSheetViewModel.ShowCountSheet(employeeId), LoadingIndicator);
        }

        private async void Form_Clicked(object sender, EventArgs e)
        {
            await _homeHelper.NavigateToModalPage(_countSheetViewModel, EmployeeDetails);
        }

        private async void OnCountSheetTapped(object sender, EventArgs e)
        {
            if (isNavigating) return;
            isNavigating = true;

            try
            {
                if (sender is BindableObject bindable && bindable.BindingContext is CountSheet selectedCountSheet)
                {
                    await _homeHelper.NavigateToCountSheetsPage(selectedCountSheet, _itemCountService, _httpClientService, EmployeeDetails);
                }
            }
            finally
            {
                isNavigating = false;
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is CountSheet selectedCountSheet)
            {
                await _editCountDialogHelper.EditCountSheetDialog(selectedCountSheet);
                LoadCountSheets();
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is CountSheet selectedCountSheet)
            {
                await _homeHelper.HandleDeleteCountSheet(selectedCountSheet, CountSheets, employeeId);
            }
        }
    }
}
