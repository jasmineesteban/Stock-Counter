namespace StockCounterBackOffice.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string GetConnectionStringParameter(string connectionString, string parameterName)
        {
            var parameters = connectionString.Split(';');
            foreach (var parameter in parameters)
            {
                if (parameter.StartsWith($"{parameterName}=", StringComparison.OrdinalIgnoreCase))
                {
                    return parameter.Substring($"{parameterName}=".Length);
                }
            }
            return null;
        }

        public static Uri GetBaseAddress(string serverName, string portNumber)
        {
          
            return new Uri($"http://{serverName}:{portNumber}");
        }

    }
}
