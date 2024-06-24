using MauiApp1.Services;
using MauiApp1.ViewModels;

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
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                await DisplayAlert("Oops", "Choose your name first.", "OK");
            }
        }
    }
}
