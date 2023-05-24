using FlightPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]

    public class AdminApiController : BaseApiController
    {
        private readonly object _locker = new object();
        public AdminApiController(FlightPlannerDbContext context) :base(context) { }

        [HttpGet]
        [Route("flights/{id}")]
        
        public IActionResult GetFlights(int id)
        {
            var flight = _context.Flights.Include(f=> f.From).Include(f => f.To).SingleOrDefault(f => f.Id == id);

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
            lock (_locker)
            {

                if (flight == null ||
                    string.IsNullOrWhiteSpace(flight.ArrivalTime) ||
                    string.IsNullOrWhiteSpace(flight.DepartureTime) ||
                    string.IsNullOrWhiteSpace(flight.Carrier) ||
                    flight.To == null ||
                    string.IsNullOrWhiteSpace(flight.To.Country) ||
                    string.IsNullOrWhiteSpace(flight.To.City) ||
                    string.IsNullOrWhiteSpace(flight.To.AirportCode) ||
                    flight.From == null ||
                    string.IsNullOrWhiteSpace(flight.From.Country) ||
                    string.IsNullOrWhiteSpace(flight.From.City) ||
                    string.IsNullOrWhiteSpace(flight.From.AirportCode) ||
                    DateTime.Parse(flight.ArrivalTime) <= DateTime.Parse(flight.DepartureTime) ||
                    string.Equals(flight.To.City, flight.From.City, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(flight.To.AirportCode, flight.From.AirportCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest(flight);
                }

                if (_context.Flights.Any(f => f.From.Country.Equals(flight.From.Country) &&
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

                _context.Flights.Add(flight);
                _context.SaveChanges();
            }
            return Created("", flight);
        }



        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flight = _context.Flights.SingleOrDefault(f => f.Id == id);

            if (_context.Flights.Contains(flight)) 
            {
                _context.Flights.Remove(flight);
                _context.SaveChanges(true);

                return Ok(flight);
            }

            return Ok();
            
        }
    }
}
