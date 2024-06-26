using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MauiApp1.Models;
using MauiApp1.Services.HttpBaseService;

namespace MauiApp1.Services
{
    public class ItemCountService : HttpServiceBase
    {
        public ItemCountService(HttpClient httpClient, TokenService tokenService)
            : base(httpClient, tokenService)
        {
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
