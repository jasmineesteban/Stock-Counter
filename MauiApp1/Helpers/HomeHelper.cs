using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Models;
using MauiApp1.Pages;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiApp1.Helpers
{
    internal class HomeHelper
    {
        private readonly CountSheetViewModel _countSheetViewModel;

        public HomeHelper(CountSheetViewModel countSheetViewModel)
        {
            _countSheetViewModel = countSheetViewModel;
        }

        public async Task NavigateToModalPage(CountSheetViewModel countSheetViewModel, string employeeDetails)
        {
            var modalPage = new ModalPage(countSheetViewModel)
            {
                EmployeeDetails = employeeDetails
            };
            await Shell.Current.Navigation.PushModalAsync(modalPage);
        }

        public async Task NavigateToCountSheetsPage(CountSheet selectedCountSheet, ItemCountService itemCountService, HttpClientService httpClientService, string employeeDetails)
        {
            var itemCountViewModel = new ItemCountViewModel(itemCountService);
            var countSheetsPage = new CountSheetsPage(itemCountViewModel, selectedCountSheet.CountCode, 0, httpClientService)
            {
                BindingContext = selectedCountSheet,
                EmployeeDetails = employeeDetails
            };
            await Shell.Current.Navigation.PushAsync(countSheetsPage);
        }

        public async Task HandleDeleteCountSheet(CountSheet selectedCountSheet, ObservableCollection<CountSheet> countSheets, string employeeId)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Delete", $"Are you sure you want to delete {selectedCountSheet.CountDescription}?", "Yes", "No");
            if (answer)
            {
                try
                {
                    await _countSheetViewModel.DeleteCountSheet(selectedCountSheet.CountCode);
                    var toast = Toast.Make($"Deleted {selectedCountSheet.CountDescription}", ToastDuration.Short);
                    await toast.Show();

                    await DataLoader.LoadDataAsync(countSheets, () => _countSheetViewModel.ShowCountSheet(employeeId), new ActivityIndicator()); // Adjust loading indicator as needed
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"Failed to delete: {ex.Message}", "OK");
                }
            }
        }
    }
}
