using MauiApp1.Models;
using Newtonsoft.Json;
using System.Text;

namespace MauiApp1.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetRemoteDatabaseAsync()
        {
            var response = await _httpClient.GetAsync("api/Database/GetRemoteDatabase");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(string databaseName, string pattern)
        {
            var response = await _httpClient.GetAsync($"api/Employee/GetEmployees?databaseName={databaseName}&pattern={pattern}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Employee>>(json);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string databaseName, string pattern)
        {
            var response = await _httpClient.GetAsync($"api/Item/GetItems?databaseName={databaseName}&pattern={pattern}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Item>>(json);
        }


        public async Task<bool> SetConnectionStringAsync(string connectionString)
        {
            var encodedConnectionString = System.Net.WebUtility.UrlEncode(connectionString);
            var content = new StringContent(JsonConvert.SerializeObject(new { ConnectionString = encodedConnectionString }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Database/SetConnectionString", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API call failed: {response.StatusCode}, {errorContent}");
                return false;
            }
            return true;
        }
    }
}
