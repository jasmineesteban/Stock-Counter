using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MauiApp1.Models;
using MauiApp1.Services.HttpBaseService;

namespace MauiApp1.Services
{
    public class CountSheetService : HttpServiceBase
    {
        public CountSheetService(HttpClient httpClient, TokenService tokenService)
            : base(httpClient, tokenService)
        {
        }

        public async Task AddCountSheetAsync(CountSheetTestModel countSheet)
        {
            // Set authorization header
            await SetAuthorizationHeaderAsync();

            var json = JsonConvert.SerializeObject(countSheet);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/CountSheet/add", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
