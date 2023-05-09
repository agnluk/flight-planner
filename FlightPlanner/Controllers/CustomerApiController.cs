using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]

    public class CustomerApiController : ControllerBase
    {

            [HttpGet]
            [Route("airports")]
            public IActionResult SearchAirports([FromQuery] string search)
            {
                var list = FlightStorage.GetAllFlights();
                var filteredAirports = new List<Airport>();

                foreach(var flight in list) 
                {
                    if (flight.From.AirportCode.Equals(search))
                        filteredAirports.Add(new Airport()
                        {
                        AirportCode = flight.From.AirportCode,
                        City = flight.From.City,
                        Country = flight.From.Country,
                        });
                }

                return Ok(filteredAirports.ToArray());
            }
    }
}


