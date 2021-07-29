using dockerapi.Models;
using dockerapi.Scripts.InformationManipulation;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections;
using System.IO;

namespace dockerapi.ConnectionService
{
    public class PostgresConnector : IConnector
    {
        private IConfiguration _config;
        private NpgsqlConnection con;
        private string SQLStatmentsLocation;

        public PostgresConnector(IConfiguration config)
        {
            this._config = config;
            this.con = ConnectionFactory.GeneratePostgresConnection(this._config);
            this.SQLStatmentsLocation = this._config.GetValue<string>("CurrentDB:QueriesLocation");
        }

        public void OpenConnection()
        {
            this.con.Open();
        }
        public void CloseConnection()
        {
            this.con.Close();
        }
        public String CheckDBVersion()
        {
            this.OpenConnection();
            String sql = "SELECT version()";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, this.con);
            String version = cmd.ExecuteScalar().ToString();
            this.CloseConnection();

            String answer = $"You are using PostgreSQL, version: {version}";
            return answer;
        }

        public Music QuerySelectRandomSong(string SQLStatementFile)
        {
            string sql = File.ReadAllText(@SQLStatementFile);
            this.OpenConnection();
            var cmd = new NpgsqlCommand(sql, this.con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();

            Music music = new();
            while (rdr.Read())
            {
                music.Band = rdr.GetString(0);
                music.Song = rdr.GetString(1);
                music.Style = rdr.GetString(2);
            }
            this.CloseConnection();
            return music;
        }

        public int CheckMusicGenresQuery(IGenresChecker genresChecker, string SQLStatementFile, string genreType)
        {
            IGenresChecker _genresChecker = genresChecker;
            string sql = File.ReadAllText(@SQLStatementFile);
            this.OpenConnection();
            var cmd = new NpgsqlCommand(sql, this.con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                _genresChecker.AddItem(item: rdr.GetString(1));
            }
            this.CloseConnection();

            int itemPosition = _genresChecker.CheckItemPosition(genreType);
            if (itemPosition == -1)
            {
                String otherCategory = "Other";
                itemPosition = _genresChecker.CheckItemPosition(item: otherCategory);
            }

            return itemPosition;
        }

        public void QueryWithParametersInsert(string SQLStatementFile, string[] ExpectedParameters, Hashtable SQLParameters, Hashtable ParameterTypes)
        {
            if (SQLParameters.Count != ParameterTypes.Count)
            {
                throw new Exception("A number of parameters and a number of indicated parameter types do not match");
            }

            this.OpenConnection();
            string sql = File.ReadAllText(SQLStatementFile);
            NpgsqlCommand cmd = new NpgsqlCommand(sql, this.con);
            for (int i = 0; i < SQLParameters.Count; i++)
            {
                string tempExpectedParameter = ExpectedParameters[i];
                var tempParameter = SQLParameters[tempExpectedParameter];
                string tempType = (string)ParameterTypes[tempExpectedParameter];

                switch (tempType)
                {
                    case "int":
                        cmd.Parameters.Add($"@{tempExpectedParameter}", NpgsqlTypes.NpgsqlDbType.Integer);
                        break;

                    case "string":
                        cmd.Parameters.Add($"@{tempExpectedParameter}", NpgsqlTypes.NpgsqlDbType.Text);
                        break;
                }
                cmd.Parameters[$"@{tempExpectedParameter}"].Value = tempParameter;
            }
            // cmd.Prepare();
            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }
}