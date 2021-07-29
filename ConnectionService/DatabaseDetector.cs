using Microsoft.Extensions.Configuration;

namespace dockerapi.ConnectionService
{
    public static class DatabaseDetector
    {
        public static IConnector FindCorrectDatabaseConnector(IConfiguration config)
        {
            IConfiguration _config = config;
            string callingMethodOfRequiredDatabase = _config.GetValue<string>("CurrentDB:UsedDB");
            IConnector connector;
            switch (callingMethodOfRequiredDatabase)
            {
                case "Postgres":
                {
                    connector = new PostgresConnector(_config);
                    break;
                }
                default:
                    // So far this is a keeper, because we do not have other databases
                    connector = new PostgresConnector(_config);
                    break;
            }
            return connector;
        }
    }
}