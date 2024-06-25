using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Item = MauiApp1.Models.Item;

namespace MauiApp1.ViewModels
{
    public partial class ItemViewModel : ObservableObject
    {
        private readonly HttpClientService _httpClientService;

        [ObservableProperty]
        private ObservableCollection<Item> _items = new ObservableCollection<Item>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNextButtonEnabled))]
        private Item _selectedItem;

        public bool IsNextButtonEnabled => SelectedItem != null;

        public ItemViewModel(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            LoadItemsCommand = new AsyncRelayCommand<string>(LoadItemsAsync);
            OnSearchBarTextChangedCommand = new AsyncRelayCommand<string>(OnSearchBarTextChanged);
            OnNextButtonClickedCommand = new AsyncRelayCommand(OnNextButtonClicked);
            LoadItemsCommand.Execute(null);
        }

        public IAsyncRelayCommand<string> LoadItemsCommand { get; }
        public IAsyncRelayCommand<string> OnSearchBarTextChangedCommand { get; }
        public IAsyncRelayCommand OnNextButtonClickedCommand { get; }

        private async Task LoadItemsAsync(string? pattern)
        {
            try
            {
                var items = await _httpClientService.GetItemsAsync(pattern);
                PopulateItemsList(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading items: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load items.", "OK");
            }
        }

        private void PopulateItemsList(IEnumerable<Item> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                if (item != null)
                {
                    Items.Add(item);
                }
            }
        }

        private async Task OnNextButtonClicked()
        {
            if (SelectedItem != null)
            {
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select an item before proceeding.", "OK");
            }
        }

        private async Task OnSearchBarTextChanged(string? newTextValue)
        {
            await LoadItemsAsync(newTextValue);
        }
    }
}
