using System.Text;
using Newtonsoft.Json;
using MauiApp1.Models;
using MauiApp1.Services.HttpBaseService;
using System.Diagnostics;

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
    }
}
