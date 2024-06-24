using MauiApp1.Services;
using MauiApp1.ViewModels;

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
    }
}
