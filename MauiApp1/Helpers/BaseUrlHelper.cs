namespace MauiApp1.Helpers
{
    public static class BaseUrlHelper
    {


        public static void ConfigureHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(10);
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
