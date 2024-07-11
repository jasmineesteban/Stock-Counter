using MauiApp1.Services;
using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace MauiApp1.Pages
{
    public partial class SignInPage : ContentPage
    {
        private readonly HttpClientService _httpClientService;
        private readonly string _fileName = "config.bgc";
        private bool _isNavigating;
        private readonly ISecurity _securityService;

        public SignInPage(HttpClientService httpClientService, ISecurity securityService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;
            _securityService = securityService;

            var items = new List<CarouselItem>
            {
                new CarouselItem { Title = "Welcome to Stock Counter", Description = "Have an effortless inventory management by keeping your stocks organized. Track everything easily!", Image = "start.png"},
                new CarouselItem { Title = "Ready to Start?", Description = "Download the config file (in Downloads) to connect to your server.", Image = "downloadfile.png"},
                new CarouselItem { Title = "Enable Barcode Searching", Description = "Grant camera permission for faster product search.", Image = "camera.jpg"}
            };

            carouselView.ItemsSource = items;
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
            var (connectionString, fromFile) = await StartupHelper.GetConnectionStringAsync(downloadPath, _fileName);

            if (string.IsNullOrEmpty(connectionString))
            {
                await DisplayAlert("Missing Connection String",
                    "No connection string found. Please check the config.bgc file or secure storage.", "OK");
                _isNavigating = false;
                return;
            }

            string decryptedConnectionString;
            if (fromFile)
            {
                decryptedConnectionString = await _securityService.DecryptAsync(connectionString);
            }
            else
            {
                decryptedConnectionString = connectionString;
            }

            string server = ConnectionStringHelper.GetServerValue(decryptedConnectionString);
            string portNumber = ConnectionStringHelper.GetPortNumber(decryptedConnectionString);
            GlobalVariable.BaseAddress = ConnectionStringHelper.GetBaseAddress(server,portNumber);

            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("No Internet Connection",
                        "You're offline. Check your connection and try again.\nHow to fix:\n� Ensure Wi-Fi or mobile data is on.\n� Verify airplane mode is off.", "OK");
                    return;
                }

                var apiResult = await _httpClientService.SetConnectionStringAsync(decryptedConnectionString);

                if (apiResult)
                {
                    await SecureStorage.SetAsync("connectionString", decryptedConnectionString);

                    if (fromFile)
                    {
                        string filePath = StartupHelper.FindFileCaseInsensitive(downloadPath, _fileName);
                        if (filePath != null)
                        {
                            File.Delete(filePath);
                        }
                    }

                    await Shell.Current.GoToAsync(nameof(EmployeeSelectorPage));
                }
                else
                {
                    await DisplayAlert("Invalid config.bgc file content",
                        "The connection string is invalid.\nHow to fix:\n" +
                        "� Ensure the config.bgc file is correctly formatted.\n" +
                        "� Double-check for any typos or missing characters.\n" +
                        "� Verify that the server address, database name, username, and password are correct.",
                        "OK");
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Host is offline or unreachable",
                    "Unable to connect to the Host.\nHow to fix:\n�Ensure the server is online.\n�Ensure the server host is running and reachable.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Unexpected Error",
                    $"An unexpected error occurred: {ex.Message}. Try again later", "OK");
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
