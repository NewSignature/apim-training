using Microsoft.AspNetCore.Mvc;

namespace PeopleApi.Controllers
{
    [ApiController]
    [Route("ping")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("I am working");
        }
    }
}