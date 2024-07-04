using MauiApp1.Services;
using MauiApp1.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1.ViewModels
{
    public class ItemViewModel2 : INotifyPropertyChanged
    {
        private readonly HttpClientService _httpClientService;

        public ItemViewModel2(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<IEnumerable<Item>> GetItem(string pattern)
        {
            return await _httpClientService.GetItemsAsync(pattern);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}