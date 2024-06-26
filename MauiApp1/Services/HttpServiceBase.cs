using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MauiApp1.Services;

namespace MauiApp1.Services.HttpBaseService
{
    public abstract class HttpServiceBase
    {
        protected readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

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
