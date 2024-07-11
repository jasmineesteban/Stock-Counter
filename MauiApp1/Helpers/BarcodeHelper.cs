using ZXing.Mobile;
using Microsoft.Maui.ApplicationModel;

namespace MauiApp1.Helpers
{
    public static class BarcodeHelper
    {
        public static async Task<string> ScanBarcodeAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status == PermissionStatus.Granted)
            {
                var options = new MobileBarcodeScanningOptions
                {
                    AutoRotate = true,
                    UseFrontCameraIfAvailable = false,
                };

                var scanner = new MobileBarcodeScanner
                {
                    TopText = "Hold the camera up to the barcode",
                    BottomText = "Scanning will happen automatically",
                };

                var result = await scanner.Scan(options);
                return result?.Text;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Permission Denied", "Camera permission is required to scan barcodes.", "OK");
                return null;
            }
        }
    }
}
