using MauiApp1.Services;
using MauiApp1.ViewModels;
using MauiApp1.Models;

namespace MauiApp1.Pages
{
    public partial class EmployeeSelectorPage : ContentPage
    {
        private readonly HttpClientService _httpClientService;
        private readonly EmployeeViewModel _viewModel;

        public EmployeeSelectorPage(HttpClientService httpClientService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;

            _viewModel = new EmployeeViewModel(httpClientService);
            BindingContext = _viewModel;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel != null && !string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                _viewModel.OnSearchBarTextChangedCommand.Execute(e.NewTextValue);
            }
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.SelectedEmployee != null)
            {
                var employeeDetails = _viewModel.SelectedEmployeeDetails;
                var navigationParameters = new Dictionary<string, object>
                {
                    { "EmployeeDetails", employeeDetails }
                };

                await Shell.Current.GoToAsync("//HomePage", navigationParameters); 
            }
            else
            {
                await DisplayAlert("Oops", "Choose your name first.", "OK");
            }
        }
        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Employee selectedEmployee)
            {
                _viewModel.SelectedEmployee = selectedEmployee;
            }
        }

    }
}
