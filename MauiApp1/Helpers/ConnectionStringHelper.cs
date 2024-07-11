using MauiApp1.Models;

namespace MauiApp1.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string GetServerValue(string connectionString)
        {
            var parameters = connectionString.Split(';');
            foreach (var parameter in parameters)
            {
                if (parameter.StartsWith("Server=", StringComparison.OrdinalIgnoreCase))
                {
                    return parameter.Substring("Server=".Length);
                }
            }
            return null;
        }

        public static string GetPortNumber(string connectionString)
        {
            var parameters = connectionString.Split(';');
            foreach (var parameter in parameters)
            {
                if (parameter.StartsWith("PortNumber=", StringComparison.OrdinalIgnoreCase))
                {
                    return parameter.Substring("PortNumber=".Length);
                }
            }
            return null;
        }

        public static Uri GetBaseAddress(string serverName, string portNumber)
        {
            return DeviceInfo.Platform == DevicePlatform.Android
                ? new Uri($"http://{serverName}:{portNumber}/")
                : new Uri($"http://{serverName}:{portNumber}/");
        }

        public static void SetGlobalBaseAddress(string connectionString)
        {
            var server = GetServerValue(connectionString);
            var portNumber = GetPortNumber(connectionString);
            GlobalVariable.BaseAddress = GetBaseAddress(server, portNumber);
        }
    }
}
