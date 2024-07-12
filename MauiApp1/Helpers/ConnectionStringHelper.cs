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
            try
            {
                string serverTemp = serverName.Trim();
                return new Uri($"http://{serverTemp}:{portNumber}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid server address.", ex);
            }
        }
    }
}
