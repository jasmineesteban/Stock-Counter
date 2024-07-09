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
        public static async Task CheckAndRequestStoragePermissions()
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

            if (Build.VERSION.SdkInt >= BuildVersionCodes.R && !Android.OS.Environment.IsExternalStorageManager)
            {
                Intent intent = new Intent(Settings.ActionManageAppAllFilesAccessPermission);
                Android.Net.Uri uri = Android.Net.Uri.FromParts("package", Android.App.Application.Context.PackageName, null);
                intent.SetData(uri);
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }
#endif

        public static async Task<string> GetDownloadPath()
        {
#if ANDROID
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
#else
            await Task.Delay(1);
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif
        }

        public static string FindFileCaseInsensitive(string directory, string fileName)
        {
            var files = Directory.GetFiles(directory);
            return files.FirstOrDefault(f => string.Equals(Path.GetFileName(f), fileName, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<(string ConnectionString, bool FromFile)> GetConnectionStringAsync(string downloadPath, string fileName)
        {
            string filePath = FindFileCaseInsensitive(downloadPath, fileName);

            if (filePath != null)
            {
                var encryptedConnectionString = await File.ReadAllTextAsync(filePath);
                return (encryptedConnectionString, true);
            }
            else
            {
                var connectionString = await SecureStorage.GetAsync("connectionString");
                return (connectionString, false);
            }
        }


    }
}
