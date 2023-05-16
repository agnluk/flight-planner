using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]

    public class CustomerApiController : BaseApiController
    {
        public CustomerApiController(FlightPlannerDbContext context) : base(context) { }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            if (search is null)
            {
                return NotFound();
            }

            search = search.Trim().ToUpper();

            var allAirports = _context.Airports;
            var searchedList = new List<Airport>();

            foreach (var airport in allAirports)
            {
                if (airport.AirportCode.ToUpper().Contains(search) ||
                    airport.City.ToUpper().Contains(search) ||
                    airport.Country.ToUpper().Contains(search))
                {
                    searchedList.Add(airport);
                }
            }
            searchedList.Distinct().ToArray();

            if (searchedList is null)
            {
                return NotFound();
            }


            return Ok(searchedList);
        }

        [HttpPost]
        [Route("flights/search")]

        public IActionResult SearchFlights(SearchFlightsRequest request)
        {
            if (request.To == null || request.From == null || string.IsNullOrEmpty(request.DepartureDate))
            {
                return BadRequest();
            }

        var matchingList = _context.Flights.Where(item =>
        item.From.City == request.From.City &&
        item.From.AirportCode == request.From.AirportCode &&
        item.From.Country == request.From.Country &&
        item.To.City == request.To.City &&
        item.To.Country == request.To.Country &&
        item.To.AirportCode == request.To.AirportCode)
            .ToList();

            var page = new PageResult()
            {
                Page = matchingList.Count(),
                TotalItems = matchingList.Count(),
                Items = matchingList.ToArray()
            };

            if (request.To.AirportCode != null && request.From.AirportCode != null && !string.IsNullOrEmpty(request.DepartureDate))
            {
                return Ok(page);
            }

            return Ok();
        }


        [HttpGet]
        [Route("flights/{id}")]

        public IActionResult FindFlights(int id)
        {
            var flight = _context.Flights.Include(f => f.From).Include(f => f.To).SingleOrDefault(f => f.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}


