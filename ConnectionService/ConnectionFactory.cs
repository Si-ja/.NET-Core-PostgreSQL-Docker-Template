using Microsoft.Extensions.Configuration;
using Npgsql;

namespace dockerapi.ConnectionService
{
    public static class ConnectionFactory
    {
        public static NpgsqlConnection GeneratePostgresConnection(IConfiguration configuration)
        {
            var _config = configuration;
            string connectionString = $"Host={_config.GetValue<string>("PostgresDB:Host")};" +
                $"port={_config.GetValue<string>("PostgresDB:port")};" +
                $"Username={_config.GetValue<string>("PostgresDB:Username")};" +
                $"Password={_config.GetValue<string>("PostgresDB:Password")};" +
                $"Database={_config.GetValue<string>("PostgresDB:Database")}";
            return new NpgsqlConnection(connectionString);
        }
    }
}