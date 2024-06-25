using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class CountSheetService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public CountSheetService(HttpClient httpClient, TokenService tokenService)
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

        public async Task<IEnumerable<CountSheet>> GetCountSheetsAsync(string pattern)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"api/CountSheet/GetCountSheets?pattern={pattern}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<CountSheet>>(json);
        }

        public async Task AddCountSheetAsync(CountSheet countSheet)
        {
            await SetAuthorizationHeaderAsync();
            var content = new StringContent(JsonConvert.SerializeObject(countSheet), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/CountSheet/add", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task EditCountSheetAsync(CountSheet countSheet)
        {
            await SetAuthorizationHeaderAsync();
            var content = new StringContent(JsonConvert.SerializeObject(countSheet), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/CountSheet/edit", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCountSheetAsync(string countSheetKey)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/CountSheet/delete/{countSheetKey}");
            response.EnsureSuccessStatusCode();
        }
    }
}
