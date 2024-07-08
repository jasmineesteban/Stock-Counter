using MauiApp1.Services;
using MauiApp1.Helpers;
using MauiApp1.Models;

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
            string encryptedConnectionString = await StartupHelper.GetConnectionStringAsync(downloadPath, _fileName);

            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("No Internet Connection",
                        "You're offline. Check your connection and try again.\nHow to fix:\n• Ensure Wi-Fi or mobile data is on.\n• Verify airplane mode is off.", "OK");
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
                        await DisplayAlert("Invalid config.bgc file content",
                            "The connection string is invalid.\nHow to fix:\n" +
                            "• Ensure the config.bgc file is correctly formatted.\n" +
                            "• Double-check for any typos or missing characters.\n" +
                            "• Verify that the server address, database name, username, and password are correct." ,
                            "OK");
                    }

                }
                else
                {
                    await DisplayAlert("Missing config.bgc file",
                        "Check the config file\nHow to fix:\n• Check the config.bgc file in the download directory.\n•Download the config.bgc file", "OK");
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Host is offline or unreachable",
                    "Unable to connect to the Host.\nHow to fix:\n•Ensure the server is online.\n•Ensure the server host is running and reachable.", "OK");
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
