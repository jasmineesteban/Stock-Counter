﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public HttpClientService(HttpClient httpClient, TokenService tokenService)
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

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(string pattern)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"api/Employee/GetEmployees?databaseName=&pattern={pattern}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Employee>>(json);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string pattern)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"api/Item/GetItems?databaseName=&pattern={pattern}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Item>>(json);
        }

        public async Task<bool> SetConnectionStringAsync(string connectionString)
        {
            await SetAuthorizationHeaderAsync();
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
