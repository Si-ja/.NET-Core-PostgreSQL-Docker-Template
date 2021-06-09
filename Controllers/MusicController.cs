using System;
using Microsoft.AspNetCore.Mvc;
using dockerapi.Scripts.DatabaseManipulations;
using dockerapi.Scripts.InformationManipulation;
using Npgsql;
using dockerapi.Models;

namespace dockerapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicController : ControllerBase
    {
        private Connector connector = new Connector();

        /// <summary>
        /// Get a random song from the Database and return to the user.
        /// </summary>
        /// <returns>A song with it's small description</returns>
        [HttpGet("random")]
        public Music GetRandomSong()
        {
            // Take the script that we need to execute to get a random song and send it to the database
            String sqlPath = @"DBScripts/RandomSong.sql";
            NpgsqlDataReader rdr = this.connector.sendSelectionQuery(path: sqlPath);

            // Create an object that will store our data (we only have 1 row-like output)
            // Go over a read object and populate information in a model that will be returned to the user
            Music music = new Music();
            while (rdr.Read())
            {
                // The sequence of what comes as 0, 1, 2 is dictated by the sequence of the columns the return gives back
                music.Band = rdr.GetString(0);
                music.Song = rdr.GetString(1);
                music.Style = rdr.GetString(2);
            }

            // Close the connection to DB
            this.connector.closeConnection();
            
            return music;
        }

        [HttpPut("style/{bandname}/{songname}/{genretype}")]
        public void PutSongIn(String bandname, String songname, String genretype)
        {
            // Take the script that will check what kind of song types do we have with our database 
            // And verify whether the user entered value is allowed 
            String sqlPath = @"DBScripts/CheckStyles.sql";
            NpgsqlDataReader rdr = this.connector.sendSelectionQuery(path: sqlPath);

            // Create a collector that can store information on all genres we have present for us
            // It's far from being efficient, but this is an example to get a point a cross...
            // Do not do it this way in production though
            Genres genres = new Genres(); 
            while (rdr.Read())
            {
                genres.AddItem(item: rdr.GetString(1));
            }
            this.connector.closeConnection();

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
            String sql = connector.readSqlStatement(path: sqlPath);
                
            // Update the parameters in it with ones the user has given in the request
            this.connector.sendMusicUpdate(sql: sql, bandname: bandname, songname: songname, genretype: itemPosition);
        }
    }
}
