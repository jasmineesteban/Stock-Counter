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
                new CarouselItem { Title = "Welcome to Stock Counter", Description = "Have and effortless inventory management by keeping your stocks organized. Track everything easily!", Image = "start.png"},
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
                        await DisplayAlert("Connection failed.", "The connection string is invalid or connection cannot be established!", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("No Connection String", "No connection string found in secure storage or config file.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
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
