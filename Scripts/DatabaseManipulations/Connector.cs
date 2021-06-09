using System;
using Npgsql;
using System.IO;

namespace dockerapi.Scripts.DatabaseManipulations
{
    public class Connector
    {
        private readonly string _connectionString;
        private NpgsqlConnection con;

        /// <summary>
        /// Generate an object for the Connector class.
        /// </summary>
        public Connector()
        {
            this._connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            this.con = new NpgsqlConnection(this._connectionString);
        }

        /// <summary>
        /// In some cases connection to DB needs to be closed manually.
        /// So do that by calling this method.
        /// </summary>
        public void closeConnection()
        {
            this.con.Close();
        }

        /// <summary>
        /// Sometimes you just have to open the closed connection.
        /// </summary>
        public void openConnection()
        {
            this.con.Open();
        }

        /// <summary>
        /// Read a sql statment for the file and return it to the user.
        /// </summary>
        /// <param name="path">Path to the file where it is located.</param>
        /// <returns>A SQL statement in a form of a string.</returns>
        public String readSqlStatement(String path)
        {
            String sql = File.ReadAllText(@path);
            return sql;
        }

        /// <summary>
        /// Check the version of the used Database.
        /// </summary>
        /// <returns>A String value indicating what version of PostgreSQL is being used.</returns>
        public String checkDBVersion()
        {
            // Open a connection and send a version check query
            this.openConnection();
            String sql = "SELECT version()";
            NpgsqlCommand cmd = new NpgsqlCommand(cmdText: sql, connection: this.con);
            String version = cmd.ExecuteScalar().ToString();
            this.closeConnection();

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
        public NpgsqlDataReader sendSelectionQuery(String path)    
        {
            // Get information from the path
            String sql = this.readSqlStatement(path: path);

            // Open a connection and send the query of interest
            this.openConnection();
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
        public void sendMusicUpdate(String sql, String bandname, String songname, int genretype)
        {
            // Open the connection and prepare our sql string with parameters that we will want to pass
            this.openConnection();
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
            this.closeConnection();
        }


    }
}