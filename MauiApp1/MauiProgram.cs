using CommunityToolkit.Maui;
using MauiApp1.Pages;
using MauiApp1.Services;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Poppins-Regular.ttf", "Poppins");
                    fonts.AddFont("Poppins-Semibold.ttf", "Poppins");
                })
                .UseMauiCommunityToolkit()
                .UseBarcodeReader();

            builder.Services.AddSingleton<SignInPage>();
            builder.Services.AddSingleton<EmployeeSelectorPage>();
            builder.Services.AddSingleton<ItemSelectorPage>();
            builder.Services.AddHttpClient<HttpClientService>(client =>
            {
                var baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                                  ? "http://192.168.254.130:7055/"
                                  : "http://localhost:7059/";

                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30); // Increase timeout to 30 seconds
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
