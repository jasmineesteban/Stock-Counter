using MauiApp1.Helpers;
using MauiApp1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace MauiApp1.Pages
{
    public partial class SignInPage : ContentPage
    {
        private readonly HttpClientService _httpClientService;
        private readonly string _fileName = "config.bgc";
        private bool _isNavigating;

        public SignInPage(HttpClientService httpClientService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (_isNavigating)
                return;

            _isNavigating = true;

#if ANDROID
            await StartupHelper.CheckAndRequestStoragePermissionsAsync();
#endif

            string encryptedConnectionString = await StartupHelper.GetConnectionStringAsync(_fileName);

            if (string.IsNullOrEmpty(encryptedConnectionString))
            {
                await DisplayAlert("No Connection String", "No connection string found in secure storage or config file.", "OK");
                _isNavigating = false;
                return;
            }

            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;

                var apiResult = await _httpClientService.SetConnectionStringAsync(encryptedConnectionString);

                if (apiResult)
                {
                    await Shell.Current.GoToAsync(nameof(EmployeeSelectorPage));
                }
                else
                {
                    await DisplayAlert("Connection failed", "The connection string is invalid or connection cannot be established!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Connection Error", $"Error using connection string: {ex.Message}", "OK");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                _isNavigating = false;
            }
        }
    }
}
