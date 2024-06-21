using MauiApp1.Services;
using MauiApp1.ViewModels;

namespace MauiApp1.Pages
{
    public partial class EmployeeSelectorPage : ContentPage
    {
        private readonly HttpClientService _httpClientService;

        public EmployeeSelectorPage(HttpClientService httpClientService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;

            var viewModel = new EmployeeViewModel(httpClientService);
            BindingContext = viewModel;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as EmployeeViewModel;
            if (viewModel != null && !string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                viewModel.OnSearchBarTextChangedCommand.Execute(e.NewTextValue);
            }
        }
    }
}
