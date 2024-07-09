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
            var baseUrl = GlobalVariable.BaseAddress.ToString();
            var json = JsonConvert.SerializeObject(itemCount);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{baseUrl}api/ItemCount/add", content);

            return response.IsSuccessStatusCode;
        }


        public async Task<IEnumerable<ItemCount>> ShowItemCountAsync(string countCode, int sortValue)
        {
            await SetAuthorizationHeaderAsync();
            var baseUrl = GlobalVariable.BaseAddress.ToString();
            var response = await _httpClient.GetAsync($"{baseUrl}api/ItemCount/show?countCode={countCode}&sort={sortValue}");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ItemCount>>(content);
        }
        public async Task<bool> EditItemCountAsync(ItemCount itemCount)
        {
            await SetAuthorizationHeaderAsync();
            var baseUrl = GlobalVariable.BaseAddress.ToString();
            var json = JsonConvert.SerializeObject(itemCount);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUrl}api/ItemCount/edit", content);
            return response.IsSuccessStatusCode;
        }

         public async Task<bool> DeleteItemCountAsync(string itemKey)
        {
            await SetAuthorizationHeaderAsync();
            var baseUrl = GlobalVariable.BaseAddress.ToString();
            var response = await _httpClient.DeleteAsync($"{baseUrl}api/ItemCount/delete?itemKey={itemKey}");

            return response.IsSuccessStatusCode;
        }
    }
}
