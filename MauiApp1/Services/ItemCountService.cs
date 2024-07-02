using System.Text;
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

        public async Task<bool> AddItemCountAsync(ItemCountAddition itemCount)
        {
            await SetAuthorizationHeaderAsync();

            var json = JsonConvert.SerializeObject(itemCount);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/ItemCount/add", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ItemCount>> ShowItemCountAsync(string countCode)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"api/ItemCount/show?countCode={countCode}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ItemCount>>(content);
        }
        public async Task<bool> EditItemCountAsync(ItemCount itemCount)
        {
            await SetAuthorizationHeaderAsync();
            var json = JsonConvert.SerializeObject(itemCount);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/ItemCount/edit", content);
            return response.IsSuccessStatusCode;
        }

         public async Task<bool> DeleteItemCountAsync(string itemKey)
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync($"api/ItemCount/delete?itemKey={itemKey}");

            return response.IsSuccessStatusCode;
        }
    }
}
