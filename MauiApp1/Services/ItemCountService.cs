using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class ItemCountService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public ItemCountService(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IEnumerable<ItemCount>> GetItemCountsAsync(string countCode)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"api/ItemCount/show/{countCode}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ItemCount>>(json);
        }

        public async Task AddItemCountAsync(ItemCount itemCount)
        {
            await SetAuthorizationHeaderAsync();
            var content = new StringContent(JsonConvert.SerializeObject(itemCount), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/ItemCount/add", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task EditItemCountAsync(ItemCount itemCount)
        {
            await SetAuthorizationHeaderAsync();
            var content = new StringContent(JsonConvert.SerializeObject(itemCount), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/ItemCount/edit", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteItemCountAsync(string itemKey)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/ItemCount/delete/{itemKey}");
            response.EnsureSuccessStatusCode();
        }
    }
}
