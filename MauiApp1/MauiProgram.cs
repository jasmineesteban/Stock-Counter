using CommunityToolkit.Maui;
using MauiApp1.Helpers;
using MauiApp1.Pages;
using MauiApp1.Services;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using Microsoft.Maui.Hosting;
using System;

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

            RegisterHttpClient<TokenService>(builder);
            RegisterHttpClient<HttpClientService>(builder);
            RegisterHttpClient<ItemCountService>(builder);
            RegisterHttpClient<CountSheetService>(builder);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterHttpClient<TClient>(MauiAppBuilder builder) where TClient : class
        {
            builder.Services.AddHttpClient<TClient>(client =>
            {
                BaseUrlHelper.ConfigureHttpClient(client);
            })
            .ConfigurePrimaryHttpMessageHandler(() => BaseUrlHelper.CreateHttpClientHandler());
        }
    }
}
