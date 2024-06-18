using MauiApp1.Services;
using MySqlConnector;
using System.Data;
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

        public SignInPage(HttpClientService httpClientService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
#if ANDROID
            await CheckAndRequestStoragePermissions();
#endif

            string downloadPath = await GetDownloadPath();
            string filePath = Path.Combine(downloadPath, _fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    var connectionString = await File.ReadAllTextAsync(filePath);
                    var validationResult = await Task.Run(() => ValidateConnectionString(connectionString));

                    if (validationResult?.StartsWith("Upload success") == true)
                    {
                        var apiResult = await _httpClientService.SetConnectionStringAsync(connectionString);
                        if (apiResult)
                        {
                            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                        }
                        else
                        {
                            await DisplayAlert("API Error", "Validation successful, but failed to send connection string to API.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Validation Error", validationResult ?? "Validation result is null", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("File Read Error", $"Error reading file: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("File Not Found", $"File not found: {filePath}", "OK");
            }
        }

        private async Task<string> GetDownloadPath()
        {
            string downloadPath;

#if ANDROID
            downloadPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
#else
                    // Placeholder for future async operation, if any
                    await Task.CompletedTask;
                    downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif

            return downloadPath;
        }



        private string ValidateConnectionString(string connStr)
        {
            try
            {
                if (!connStr.Contains("Connection Timeout", StringComparison.OrdinalIgnoreCase))
                {
                    connStr += ";Connection Timeout=30;";
                }

                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open ? "Upload success. Connected to the database." : "Failed to connect to the database.";
                }
            }
            catch (MySqlException ex)
            {
                return $"MySQL error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
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
                    intent.AddFlags(ActivityFlags.NewTask); // Add this line to set the FLAG_ACTIVITY_NEW_TASK flag
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
        }
#endif
    }
}
