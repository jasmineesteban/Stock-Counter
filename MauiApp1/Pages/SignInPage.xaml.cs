using MauiApp1.Services;
using MauiApp1.Helpers;
using System.Net.Http;

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
            await StartupHelper.CheckAndRequestStoragePermissions();
#endif

            string downloadPath = await StartupHelper.GetDownloadPath();
            string encryptedConnectionString = await StartupHelper.GetConnectionStringAsync(downloadPath, _fileName);

            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("No Internet Connection",
                        "You're offline. Check your connection and try again.\nHow to fix:\n1. Ensure Wi-Fi or mobile data is on.\n2. Verify airplane mode is off.", "OK");
                    return;
                }

                if (!string.IsNullOrEmpty(encryptedConnectionString))
                {
                    var apiResult = await _httpClientService.SetConnectionStringAsync(encryptedConnectionString);

                    if (apiResult)
                    {
                        await SecureStorage.SetAsync("connectionString", encryptedConnectionString);
                        string filePath = StartupHelper.FindFileCaseInsensitive(downloadPath, _fileName);
                        if (filePath != null)
                        {
                            File.Delete(filePath);
                        }

                        await Shell.Current.GoToAsync(nameof(EmployeeSelectorPage));
                    }
                    else
                    {
                        await DisplayAlert("Invalid Connection String",
                            "The connection string is invalid. Please verify it.\nHow to fix:\n1. Ensure the string is correctly formatted.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Missing config.bgc file",
                        "Check the config file\nHow to fix:\n1. Check the config.bgc file in the download directory.\n2. Verify the file content.", "OK");
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("API Connection Failed",
                    "Unable to connect to the API. Please ensure the server is running and reachable\nHow to fix:\n1. Ensure the server is online.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Unexpected Error",
                    $"An unexpected error occurred: {ex.Message}. Try again or contact support.", "OK");
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
