using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PeopleApi.Controllers
{
    [ApiController]
    [Route("ping")]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <response code="200">Successfully responded</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Ping()
        {
            return Ok(_configuration["API_VERSION"] ?? "No Version");
        }
    }
}