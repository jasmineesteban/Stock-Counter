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


        private readonly HttpClientService _httpClientService;
        private readonly EditCountDialogHelper _editCountDialogHelper;

        public HomePage(CountSheetViewModel countSheetViewModel, ItemCountService itemCountService, HttpClientService httpClientService)
        {
            InitializeComponent();
            _countSheetViewModel = countSheetViewModel;
            _itemCountService = itemCountService;
            BindingContext = _countSheetViewModel;
            BindingContext = this;
            _httpClientService = httpClientService;
            _editCountDialogHelper = new EditCountDialogHelper(_countSheetViewModel);
        }

        private async void LoadCountSheets()
        {
            await DataLoader.LoadDataAsync(CountSheets, () => _countSheetViewModel.ShowCountSheet(employeeId), LoadingIndicator);
        }


        private async void Form_Clicked(object sender, EventArgs e)
        {
            var modalPage = new ModalPage(_countSheetViewModel)
            {
                EmployeeDetails = EmployeeDetails
            };
            await Shell.Current.Navigation.PushModalAsync(modalPage);
        }

        private bool isNavigating = false;

        private async void OnCountSheetTapped(object sender, EventArgs e)
        {
            if (isNavigating) return; 
            isNavigating = true;

            try
            {
                if (sender is BindableObject bindable && bindable.BindingContext is CountSheet selectedCountSheet)
                {
                    var grid = (Grid)bindable;
                    grid.BackgroundColor = Colors.LightGray;

                  
                    await Task.Delay(500);
                    grid.BackgroundColor = Colors.White;

                    var itemCountViewModel = new ItemCountViewModel(_itemCountService);
                    var countSheetsPage = new CountSheetsPage(itemCountViewModel, selectedCountSheet.CountCode, 0, _httpClientService)
                    {
                        BindingContext = selectedCountSheet,
                        EmployeeDetails = this.EmployeeDetails
                    };
                    await Shell.Current.Navigation.PushAsync(countSheetsPage);
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
                bool answer = await DisplayAlert("Delete", $"Are you sure you want to delete {selectedCountSheet.CountDescription}?", "Yes", "No");
                if (answer)
                {
                    try
                    {
                        await _countSheetViewModel.DeleteCountSheet(selectedCountSheet.CountCode);
                        var toast = Toast.Make($"Deleted {selectedCountSheet.CountDescription}", ToastDuration.Short);
                        await toast.Show();

                        LoadCountSheets(); 
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
