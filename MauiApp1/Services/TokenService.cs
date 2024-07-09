using MauiApp1.Models;
using MauiApp1.Helpers;
using Newtonsoft.Json;

namespace MauiApp1.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_token))
            {
                var baseUrl = GlobalVariable.BaseAddress.ToString();
                var response = await _httpClient.PostAsync($"{baseUrl}api/Token/generate", null);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                _token = JsonConvert.DeserializeObject<TokenResponse>(json)?.Token;
            }
            return _token;
        }
    }
}
