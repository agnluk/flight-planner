using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminApiController : ControllerBase
    {
        private readonly object _locker = new object();

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight([FromBody] Flight flight)
        {
            lock (_locker)
            {
                var listOfFlights = FlightStorage.GetAllFlights().ToList();

                if (FlightStorage.IsValidFlight(flight))
                {
                    return BadRequest(flight);
                }

                bool hasConflict = listOfFlights.Any(f => f.From.Country.Equals(flight.From.Country) &&
                                               f.To.Country.Equals(flight.To.Country) &&
                                               f.From.AirportCode.Equals(flight.From.AirportCode) &&
                                               f.To.AirportCode.Equals(flight.To.AirportCode) &&
                                               f.From.City.Equals(flight.From.City) &&
                                               f.To.City.Equals(flight.To.City) &&
                                               f.ArrivalTime.Equals(flight.ArrivalTime) &&
                                               f.DepartureTime.Equals(flight.DepartureTime) &&
                                               f.Carrier.Equals(flight.Carrier));

                if (hasConflict)
                {
                    return Conflict(flight);
                }


                FlightStorage.AddFlight(flight);
            }

            return Created("", flight);
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var listOfFlights = FlightStorage.GetAllFlights().ToList();
            var flightToRemove = listOfFlights.FirstOrDefault(f => f.Id == id);

            if (flightToRemove != null)
            {
                listOfFlights.Remove(flightToRemove);
                FlightStorage.UpdateFlights(listOfFlights);

                return Ok(flightToRemove);
            }

            return Ok();
        }
    }
}
