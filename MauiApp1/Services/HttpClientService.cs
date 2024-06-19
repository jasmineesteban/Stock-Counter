using System.Text;
using System.Text.Json;

namespace MauiApp1.Services
{
    public class HttpClientService
    {

        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> SetConnectionStringAsync(string connectionString)
        {
            try
            {
                // URL encode the connection string
                string encodedConnectionString = System.Net.WebUtility.UrlEncode(connectionString);

                var content = new StringContent(JsonSerializer.Serialize(new { ConnectionString = encodedConnectionString }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Database/SetConnectionString", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API call failed: {response.StatusCode}, {errorContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in SetConnectionStringAsync: {ex.Message}");
                return false;
            }
        }
    }
}
