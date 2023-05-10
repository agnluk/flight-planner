using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]

    public class CustomerApiController : ControllerBase
    {

        [HttpGet]
        [Route("airports")]
        public IActionResult searchAirports(string search)
        {
            var airports = FlightStorage.GetAirports();
            var listAirports = new List<Airport>();

            foreach (var airport in airports)
            {
                search = search.Replace(" ", "").ToUpper();

                if (airport.AirportCode.ToUpper().Contains(search) ||
                    airport.City.ToUpper().Contains(search) ||
                    airport.Country.ToUpper().Contains(search))
                {
                    listAirports.Add(airport);
                }
            }
            var returnList = listAirports.ToArray();

            if (returnList is null) 
            {
                return NotFound();
            }
            return Ok(returnList);
        }
    }
}


