using System;
using Npgsql;
using System.IO;
using dockerapi.ConnectionService;
using Microsoft.Extensions.Configuration;

namespace dockerapi.Scripts.DatabaseManipulations
{
    public class Connector
    {
        private IConfiguration _config;
        private readonly string _connectionString;
        private NpgsqlConnection con;

        public Connector(IConfiguration config)
        {
            this._config = config;
            this.con = ConnectionFactory.GeneratePostgresConnection(this._config);
            // this._connectionString = "Host=localhost;port=5432;;Username=root;Password=root;Database=root"; //Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            // this.con = new NpgsqlConnection(this._connectionString);
        }

        public void CloseConnection()
        {
            this.con.Close();
        }

        public void OpenConnection()
        {
            this.con.Open();
        }

        public String ReadSqlStatement(String path)
        {
            String sql = File.ReadAllText(@path);
            return sql;
        }

        public String CheckDBVersion()
        {
            // Open a connection and send a version check query
            this.OpenConnection();
            String sql = "SELECT version()";
            NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: this.con);
            String version = cmd.ExecuteScalar().ToString();
            this.CloseConnection();

            // Prepare the answer and return it to the user
            String answer = $"PostgreSQL version: {version}";
            return answer;
        }   

        /// <summary>
        /// Send a selection query to the database and return
        /// A response to the user. Which will have to be partially
        /// processed of course.
        /// </summary>
        /// <param name="path">Path to where the .sql file with the query is located.</param>
        /// <returns>A NpgsqlDataReader object with user's response</returns>
        public NpgsqlDataReader SendSelectionQuery(String path)    
        {
            // Get information from the path
            String sql = this.ReadSqlStatement(path: path);

            // Open a connection and send the query of interest
            this.OpenConnection();
            var cmd = new NpgsqlCommand(cmdText: sql, connection: con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();

            return rdr;
        }

        /// <summary>
        /// Send a query that inserts a new song to the overall collection. Original sql statement is required
        /// to be passed with several additional arguments that will change the variables in the query.
        /// </summary>
        /// <param name="sql">Original statement with place holder variables for the remaining arguments.</param>
        /// <param name="bandname">The name of the band indicated by the user.</param>
        /// <param name="songname">The name of the song indicated by the user.</param>
        /// <param name="genretype">The type of a song, verified by us.</param>
        public void SendMusicUpdate(String sql, String bandname, String songname, int genretype)
        {
            // Open the connection and prepare our sql string with parameters that we will want to pass
            this.OpenConnection();
            NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: this.con);
            cmd.Parameters.Add(parameterName: "@bandname", parameterType: NpgsqlTypes.NpgsqlDbType.Text);
            cmd.Parameters.Add(parameterName: "@songname", parameterType: NpgsqlTypes.NpgsqlDbType.Text);
            cmd.Parameters.Add(parameterName: "@genretype", parameterType: NpgsqlTypes.NpgsqlDbType.Integer);

            // Set the parameters to what the user passed them to be (no sanitization is done here. Be careful)
            cmd.Parameters["@bandname"].Value = bandname;
            cmd.Parameters["@songname"].Value = songname;
            cmd.Parameters["@genretype"].Value = genretype;

            // Sent the the query to the database
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            // Close the connection
            this.CloseConnection();
        }


    }
}