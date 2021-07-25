using System;
using Microsoft.AspNetCore.Mvc;
using dockerapi.Scripts.DatabaseManipulations;
using Microsoft.Extensions.Configuration;
using dockerapi.ConnectionService;
using System.Reflection;
using Npgsql;

namespace dockerapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private IConfiguration _config;
        private IConnector connector;

        public DatabaseController(IConfiguration configuration)
        {
            this._config = configuration;
            this.connector = DatabaseDetector.FindCorrectDatabaseConnector(this._config);
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            String answer = this.connector.CheckDBVersion();
            return answer;
        }
    }
}
