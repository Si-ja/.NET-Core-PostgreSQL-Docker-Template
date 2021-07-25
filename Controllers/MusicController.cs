using System;
using Microsoft.AspNetCore.Mvc;
using dockerapi.Scripts.InformationManipulation;
using dockerapi.Models;
using Microsoft.Extensions.Configuration;
using dockerapi.ConnectionService;
using System.Collections;

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

        [HttpGet("random")]
        public Music GetRandomSong()
        {
            // Take the script that we need to execute to get a random song and send it to the database
            String sqlPath = $@"{this._config.GetValue<string>("CurrentDB:QueriesLocation")}RandomSong.sql";
            return this.connector.QuerySelectRandomSong(sqlPath);

        }

        [HttpPut("insert/bandname={bandname}&songname={songname}&genretype={genretype}")]
        public void PutSongIn([FromServices] IGenresChecker genresChecker, String bandname, String songname, String genretype)
        {
            var _genresChecker = genresChecker;
            // First check the style we have to work with
            string sqlPath = $@"{this._config.GetValue<string>("CurrentDB:QueriesLocation")}CheckStyles.sql";
            int itemPosition = this.connector.CheckMusicGenresQuery(_genresChecker, sqlPath, genretype);
            string[] expectedParameters = { "bandname", "songname", "genretype" };

            Hashtable SQLParameters = new();
            SQLParameters.Add("bandname", bandname);
            SQLParameters.Add("songname", songname);
            SQLParameters.Add("genretype", itemPosition);

            Hashtable ParameterTypes = new();
            ParameterTypes.Add("bandname", "string");
            ParameterTypes.Add("songname", "string");
            ParameterTypes.Add("genretype", "int");

            sqlPath = $@"{this._config.GetValue<string>("CurrentDB:QueriesLocation")}InsertSong.sql";
            this.connector.QueryWithParametersInsert(sqlPath, expectedParameters, SQLParameters, ParameterTypes);
        }
    }
}
