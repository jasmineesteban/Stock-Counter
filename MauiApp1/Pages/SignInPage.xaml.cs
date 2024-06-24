using MauiApp1.Services;
#if ANDROID
using Android.Content;
using Android.OS;
using Android.Provider;
#endif

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
            await CheckAndRequestStoragePermissions();
#endif

            string downloadPath = await GetDownloadPath();
            string filePath = Path.Combine(downloadPath, _fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    LoadingIndicator.IsVisible = true;
                    LoadingIndicator.IsRunning = true;

                    var encryptedConnectionString = await File.ReadAllTextAsync(filePath);
                    var apiResult = await _httpClientService.SetConnectionStringAsync(encryptedConnectionString);

                    if (apiResult)
                    {
                        await Shell.Current.GoToAsync(nameof(EmployeeSelectorPage));
                    }
                    else
                    {
                        await DisplayAlert("Connection failed.", "The file is invalid or connection cannot be established!", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("File Read Error", $"Error reading file: {ex.Message}", "OK");
                }
                finally
                {
                    LoadingIndicator.IsVisible = false;
                    LoadingIndicator.IsRunning = false;
                }
            }
            else
            {
                await DisplayAlert("File Not Found", $"File not found: {filePath}", "OK");
            }

            _isNavigating = false;
        }


        private async Task<string> GetDownloadPath()
        {
            string downloadPath;

#if ANDROID
            downloadPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
#else
    await Task.Delay(1); // Dummy await
    downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif

            return downloadPath;
        }


#if ANDROID
        private async Task CheckAndRequestStoragePermissions()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                if (!Android.OS.Environment.IsExternalStorageManager)
                {
                    Intent intent = new Intent();
                    intent.SetAction(Settings.ActionManageAppAllFilesAccessPermission);
                    Android.Net.Uri uri = Android.Net.Uri.FromParts("package", Android.App.Application.Context.PackageName, null);
                    intent.SetData(uri);
                    intent.AddFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
        }
#endif

    }
}
