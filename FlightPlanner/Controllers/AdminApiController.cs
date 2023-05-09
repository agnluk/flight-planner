using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        [HttpGet]
        [Route("flights/{id}")]
        
        public IActionResult GetFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if(flight == null) 
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(Flight flight)
        {
            var listOfFlights = FlightStorage.GetAllFlights();

            if (listOfFlights.Any(f => f.ArrivalTime == flight.ArrivalTime &&
                                        f.DepartureTime == flight.DepartureTime &&
                                        f.Carrier == flight.Carrier))
            {
                return Conflict(flight);
            }
            else if (flight == null ||
                     string.IsNullOrEmpty(flight.ArrivalTime) ||
                     string.IsNullOrEmpty(flight.DepartureTime) ||
                     string.IsNullOrEmpty(flight.Carrier) ||
                     flight.To is null ||
                     string.IsNullOrEmpty(flight.To.Country) ||
                     string.IsNullOrEmpty(flight.To.City) ||
                     string.IsNullOrEmpty(flight.To.AirportCode) ||
                     flight.From is null ||
                     string.IsNullOrEmpty(flight.From.Country) ||
                     string.IsNullOrEmpty(flight.From.City) ||
                     string.IsNullOrEmpty(flight.From.AirportCode) ||
                     string.Equals(flight.To.Country, flight.From.Country, StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(flight.To.City, flight.From.City, StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(flight.To.AirportCode, flight.From.AirportCode, StringComparison.OrdinalIgnoreCase) ||
                     DateTime.Parse(flight.ArrivalTime) < DateTime.Parse(flight.DepartureTime) ||
                     DateTime.Parse(flight.ArrivalTime) == DateTime.Parse(flight.DepartureTime)
                     )
            {
                return BadRequest(flight);
            }
            else
            {
                FlightStorage.AddFlight(flight);

                return Created("", flight);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flight = FlightStorage.DeleteFlight(id);
             if (flight is null) 
            {
                return Ok();
            }

            return Ok(flight);
        }
    }
}
