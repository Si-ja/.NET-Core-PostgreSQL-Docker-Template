using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace dockerapi.ConnectionService
{
    public static class ConnectionFactory
    {
        public static NpgsqlConnection GeneratePostgresConnection(IConfiguration configuration)
        {
            var _config = configuration;
            string connectionString = $"Host={Environment.GetEnvironmentVariable("APPSETTINGS_HOST") ?? _config.GetValue<string>("PostgresDB:Host")};" +
                $"port={Environment.GetEnvironmentVariable("APPSETTINGS_PORT") ?? _config.GetValue<string>("PostgresDB:port")};" +
                $"Username={Environment.GetEnvironmentVariable("APPSETTINGS_USERNAME") ?? _config.GetValue<string>("PostgresDB:Username")};" +
                $"Password={Environment.GetEnvironmentVariable("APPSETTINGS_PASSWORD") ?? _config.GetValue<string>("PostgresDB:Password")};" +
                $"Database={Environment.GetEnvironmentVariable("APPSETTINGS_DATABASE") ?? _config.GetValue<string>("PostgresDB:Database")}";
            return new NpgsqlConnection(connectionString);
        }
    }
}