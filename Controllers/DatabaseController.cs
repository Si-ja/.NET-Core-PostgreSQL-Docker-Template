using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using dockerapi.ConnectionService;

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

        /// <summary>
        /// Check to which type of a Database you are connected if any. More of a Sanity Check to see
        /// if your set up is done properly.
        /// </summary>
        /// <returns>A message indicating what connection you are holding to what database.</returns>
        [HttpGet]
        public ActionResult<string> Get()
        {
            String answer = this.connector.CheckDBVersion();
            return answer;
        }
    }
}
