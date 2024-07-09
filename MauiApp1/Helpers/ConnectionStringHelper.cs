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
    }
}
