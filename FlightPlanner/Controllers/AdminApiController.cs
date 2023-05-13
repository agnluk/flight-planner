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
        public IActionResult AddFlight([FromBody] Flight flight)
        {
            var listOfFlights = FlightStorage.GetAllFlights();
            var flightsCopy = listOfFlights.ToList();

            if (!FlightStorage.IsValidFlight(flight))
            {
                return BadRequest(flight);
            }

            if (flightsCopy.Any(f => f.From.Country.Equals(flight.From.Country) &&
                                     f.To.Country.Equals(flight.To.Country) &&
                                     f.From.AirportCode.Equals(flight.From.AirportCode) &&
                                     f.To.AirportCode.Equals(flight.To.AirportCode) &&
                                     f.From.City.Equals(flight.From.City) &&
                                     f.To.City.Equals(flight.To.City) &&
                                     f.ArrivalTime.Equals(flight.ArrivalTime) &&
                                     f.DepartureTime.Equals(flight.DepartureTime) &&
                                     f.Carrier.Equals(flight.Carrier)))
            {
                return Conflict(flight);
            }

            FlightStorage.AddFlight(flight);

            return Created("", flight);
        }


        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flight = new Flight();
            var list = FlightStorage.GetAllFlights();
            var getFlight = FlightStorage.GetFlight(id);

            if(list.Contains(getFlight)) 
            {
                flight = FlightStorage.DeleteFlight(id);

                return Ok(flight);
            }

            return Ok();
            
        }
    }
}
