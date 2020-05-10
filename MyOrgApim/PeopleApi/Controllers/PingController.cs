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

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(_configuration["API_VERSION"] ?? "No Version");
        }
    }
}