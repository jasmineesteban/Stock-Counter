﻿using System.Text;
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

        public async Task AddCountSheetAsync(CountSheetAddition countSheet)
        {
            await SetAuthorizationHeaderAsync();

            var json = JsonConvert.SerializeObject(countSheet);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/CountSheet/add", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<CountSheet>> ShowCountSheetAsync(string employeeId)
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.GetAsync($"api/CountSheet/show?employeeId={employeeId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<CountSheet>>(content);
        }
        public async Task EditCountSheetAsync(CountSheet countSheet)
        {
            await SetAuthorizationHeaderAsync();

            var json = JsonConvert.SerializeObject(countSheet);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/CountSheet/edit", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCountSheetAsync(string countCode)
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync($"api/CountSheet/delete/{countCode}");
            response.EnsureSuccessStatusCode();
        }
    }
}