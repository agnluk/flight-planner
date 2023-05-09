using FlightPlanner.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupController : ControllerBase
    {
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear() 
        {
            return Ok(FlightStorage.FlightsClear());
        }
    }
}
