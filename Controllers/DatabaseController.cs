using System;
using Microsoft.AspNetCore.Mvc;
using dockerapi.Scripts.DatabaseManipulations;

namespace dockerapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private Connector connector = new Connector();

        [HttpGet]
        public ActionResult<string> Get()
        {
            String answer = this.connector.checkDBVersion();
            return answer;
        }
    }
}
