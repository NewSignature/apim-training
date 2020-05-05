using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresidentsApi.Services;

namespace PresidentsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PresidentController : ControllerBase
    {
        private readonly PresidentDataService _presidentDataService;

        public PresidentController(PresidentDataService presidentDataService)
        {
            _presidentDataService = presidentDataService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_presidentDataService.GetAllPresidents());
        }

        [HttpGet("party/{party}")]
        public IActionResult Get(string party)
        {
            return Ok(_presidentDataService.GetAllPresidentsByParty(party));
        }

        [HttpGet("alive")]
        public IActionResult GetAlive()
        {
            return Ok(_presidentDataService.GetLivingPresidents());
        }

        [HttpGet("diedinoffice")]
        public IActionResult GetDiedInOffice()
        {
            return Ok(_presidentDataService.GetPresidentsWhoDiedInOffice());
        }
    }
}
