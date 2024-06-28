// Helpers/StartupHelper.cs
#if ANDROID
using Android.Content;
using Android.OS;
using Android.Provider;
#endif
namespace MauiApp1.Helpers
{
    public static class StartupHelper
    {
#if ANDROID
        public static async Task CheckAndRequestStoragePermissionsAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R)
            {
                if (!Android.OS.Environment.IsExternalStorageManager)
                {
                    var intent = new Intent();
                    intent.SetAction(Settings.ActionManageAppAllFilesAccessPermission);
                    var uri = Android.Net.Uri.FromParts("package", Android.App.Application.Context.PackageName, null);
                    intent.SetData(uri);
                    intent.AddFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
        }
#endif

        public static async Task<string> GetConnectionStringAsync(string fileName)
        {
            string encryptedConnectionString = await SecureStorage.GetAsync("connectionString");

            if (string.IsNullOrEmpty(encryptedConnectionString))
            {
                await TransferConfigFileToSecureStorageAsync(fileName);
                encryptedConnectionString = await SecureStorage.GetAsync("connectionString");
            }

            string downloadPath = await GetDownloadPathAsync();
            string filePath = Path.Combine(downloadPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
           // SecureStorage.Remove("connectionString");
            return encryptedConnectionString;
        }

        private static async Task TransferConfigFileToSecureStorageAsync(string fileName)
        {
            string downloadPath = await GetDownloadPathAsync();
            string filePath = Path.Combine(downloadPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    string encryptedConnectionString = await System.IO.File.ReadAllTextAsync(filePath);
                    await SecureStorage.SetAsync("connectionString", encryptedConnectionString);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error reading file: {ex.Message}");
                }
            }
        }

        public static async Task<string> GetDownloadPathAsync()
        {
#if ANDROID
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
#else
            await Task.Delay(1);
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
#endif
        }
    }
}
