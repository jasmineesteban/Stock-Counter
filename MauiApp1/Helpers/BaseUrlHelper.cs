namespace MauiApp1.Helpers
{
    public static class BaseUrlHelper
    {
        public static Uri GetBaseAddress()
        {
            return DeviceInfo.Platform == DevicePlatform.Android
                ? new Uri("http://192.168.254.130:7055/")
                : new Uri("http://localhost:7059/");
        }

        public static void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = GetBaseAddress();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromHours(1);
        }

        public static HttpClientHandler CreateHttpClientHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
        }
    }
}
