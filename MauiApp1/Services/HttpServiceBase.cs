using System.Net.Http.Headers;

namespace MauiApp1.Services.HttpBaseService
{
    public abstract class HttpServiceBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly TokenService _tokenService; // Change to protected

        protected HttpServiceBase(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        protected async Task SetAuthorizationHeaderAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
