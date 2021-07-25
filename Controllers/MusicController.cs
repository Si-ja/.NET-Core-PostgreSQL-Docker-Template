using System;
using Microsoft.AspNetCore.Mvc;
using dockerapi.Scripts.DatabaseManipulations;
using dockerapi.Scripts.InformationManipulation;
using Npgsql;
using dockerapi.Models;
using Microsoft.Extensions.Configuration;
using dockerapi.ConnectionService;

namespace dockerapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicController : ControllerBase
    {
        private IConfiguration _config;
        private IConnector connector;

        public MusicController(IConfiguration configuration)
        {
            this._config = configuration;
            this.connector = DatabaseDetector.FindCorrectDatabaseConnector(this._config);
        }

        /// <summary>
        /// Get a random song from the Database and return to the user.
        /// </summary>
        /// <returns>A song with it's small description</returns>
        [HttpGet("random")]
        public Music GetRandomSong()
        {
            // Take the script that we need to execute to get a random song and send it to the database
            String sqlPath = $@"{this._config.GetValue<string>("CurrentDB:QueriesLocation")}RandomSong.sql";
            return this.connector.QuerySelectRandomSong(sqlPath);

        }

        [HttpGet("insert/bandname={bandname}&songname={songname}&genretype={genretype}")]
        public string PutSongIn(String bandname, String songname, String genretype)
        {
            return $"The song is: {songname} by {bandname}";

            // First check the style we have to work with
            string sqlPath = $@"{this._config.GetValue<string>("CurrentDB:QueriesLocation")}CheckStyles.sql";
            this.connector.OpenConnection();

            /*
            // Take the script that will check what kind of song types do we have with our database 
            // And verify whether the user entered value is allowed 
            String sqlPath = @"DBScripts/CheckStyles.sql";
            NpgsqlDataReader rdr = this.connector.SendSelectionQuery(path: sqlPath);

            // Create a collector that can store information on all genres we have present for us
            // It's far from being efficient, but this is an example to get a point a cross...
            // Do not do it this way in production though
            Genres genres = new Genres(); 
            while (rdr.Read())
            {
                genres.AddItem(item: rdr.GetString(1));
            }
            this.connector.CloseConnection();

            // Now that we have all of our genres, we can check whether the user specified item is in it
            int itemPosition = genres.CheckItemPosition(item: genretype);

            // If it is equal to -1, then we will assign it to the "other" category
            if (itemPosition == -1)
            {
                String otherCategory = "Other";
                itemPosition = genres.CheckItemPosition(item: otherCategory);
            }

            // Now that we have our data - push it into the database
            // Get a SQL file that can be read with parameters population principle
            sqlPath = @"DBScripts/InsertSong.sql";
            String sql = connector.ReadSqlStatement(path: sqlPath);
                
            // Update the parameters in it with ones the user has given in the request
            this.connector.SendMusicUpdate(sql: sql, bandname: bandname, songname: songname, genretype: itemPosition);
            */
        }
    }
}
