using MauiApp1.Services;
using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Interfaces;
using StockCounterBackOffice.Helpers;

namespace MauiApp1.Pages
{
    public partial class SignInPage : ContentPage
    {
        private readonly HttpClientService _httpClientService;
        private readonly ISecurity _securityService;
        private readonly string _fileName = "config.bgc";
        private bool _isNavigating;

        public SignInPage(HttpClientService httpClientService, ISecurity securityService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;
            _securityService = securityService;

            InitializeCarouselItems();
        }

        private void InitializeCarouselItems()
        {
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

            var downloadPath = await StartupHelper.GetDownloadPath();
            var (connectionString, fromFile) = await StartupHelper.GetConnectionStringAsync(downloadPath, _fileName);

            if (string.IsNullOrEmpty(connectionString))
            {
                await StartupHelper.ShowAlert(this, "Missing config.bgc file", "No connection string found. Please check the config.bgc file or secure storage.", "OK");
                _isNavigating = false;
                return;
            }

            var decryptedConnectionString = fromFile
                ? await _securityService.DecryptAsync(connectionString)
                : connectionString;

            var server = ConnectionStringHelper.GetConnectionStringParameter(connectionString, "Server");
            var portNumber = ConnectionStringHelper.GetConnectionStringParameter(connectionString, "PortNumber");

            GlobalVariable.BaseAddress = ConnectionStringHelper.GetBaseAddress(server, portNumber);


            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await StartupHelper.ShowAlert(this, "No Internet Connection", "You're offline. Check your connection and try again.\nHow to fix:\n• Ensure Wi-Fi or mobile data is on.\n• Verify airplane mode is off.", "OK");
                    return;
                }

                ShowLoadingIndicator(true);

                var apiResult = await _httpClientService.SetConnectionStringAsync(decryptedConnectionString);

                if (apiResult)
                {
                    await SecureStorage.SetAsync("connectionString", decryptedConnectionString);
                    await StartupHelper.DeleteConfigFileIfNeeded(downloadPath, _fileName, fromFile);
                    await Shell.Current.GoToAsync(nameof(EmployeeSelectorPage));
                }
                else
                {
                    await StartupHelper.ShowAlert(this, "Invalid config.bgc file content", "The connection string is invalid.\nHow to fix:\n• Ensure the config.bgc file is correctly formatted.\n• Double-check for any typos or missing characters.\n• Verify that the server address, database name, username, and password are correct.", "OK");
                }
            }
            catch (HttpRequestException ex)
            {
                string errorDetails = $"Error: {ex.Message}\nInner Exception: {ex.InnerException?.Message}\nStack Trace: {ex.StackTrace}";
                await StartupHelper.ShowAlert(this, "Host is offline or unreachable", $"Unable to connect to the Host.\nHow to fix:\n• Ensure the server is online.\n• Ensure the server host is running and reachable.\n\nError Details:\n{errorDetails}", "OK");
            }
            catch (Exception ex)
            {
                await StartupHelper.ShowAlert(this, "Unexpected Error", $"An unexpected error occurred: {ex.Message}. Try again later", "OK");
            }
            finally
            {
                ShowLoadingIndicator(false);
                _isNavigating = false;
            }
        }

        private void ShowLoadingIndicator(bool show)
        {
            LoadingIndicator.IsVisible = show;
            LoadingIndicator.IsRunning = show;
        }
    }
}
