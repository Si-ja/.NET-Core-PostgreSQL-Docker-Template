using dockerapi.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace dockerapi.ConnectionService
{
    public class PostgresConnector : IConnector
    {
        private IConfiguration _config;
        private NpgsqlConnection con;
        private string SQLStatmentsLocation;
        private Hashtable TypeReferences = new Hashtable();

        public PostgresConnector(IConfiguration config)
        {
            this._config = config;
            this.con = ConnectionFactory.GeneratePostgresConnection(this._config);
            this.SQLStatmentsLocation = this._config.GetValue<string>("CurrentDB:QueriesLocation");

            // Creating a types references for the future use of the system
            this.TypeReferences.Add("int", NpgsqlTypes.NpgsqlDbType.Integer);
            this.TypeReferences.Add("string", NpgsqlTypes.NpgsqlDbType.Text);
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
            String sql = File.ReadAllText(@SQLStatementFile);
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

        public void QueryWithParametersInsert(string SQLStatementFile, string[] ExpectedParameters, Hashtable SQLParameters, Hashtable ParameterTypes)
        {
            if (SQLParameters.Count != ParameterTypes.Count)
            {
                throw new Exception("A number of parameters and a number of indicated parameter types do not match");
            }

            this.OpenConnection();
            String sql = File.ReadAllText(SQLStatementFile);
            NpgsqlCommand cmd = new NpgsqlCommand(SQLStatementFile, this.con);
            for (int i = 0; i < SQLParameters.Count - 1; i++)
            {
                string tempExpectedParameter = ExpectedParameters[i];
                string tempParameter = (string)SQLParameters[tempExpectedParameter];
                string tempType = (string)ParameterTypes[tempExpectedParameter];

                cmd.Parameters.Add($"@{tempExpectedParameter}", (NpgsqlTypes.NpgsqlDbType)this.TypeReferences[tempType]);
                cmd.Parameters[$"@{tempExpectedParameter}"].Value = tempParameter;
            }
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            this.CloseConnection();
        }
    }
}