using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
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
  

    public class CustomerApiController : ControllerBase
    {

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            if (search is null)
            {
                return NotFound();
            }

            var airports = FlightStorage.GetAirports();
            var listAirports = new List<Airport>();

            search = search.Trim().ToUpper();

            foreach (var airport in airports)
            {
                if (airport.AirportCode.ToUpper().Contains(search) ||
                    airport.City.ToUpper().Contains(search) ||
                    airport.Country.ToUpper().Contains(search))
                {
                    listAirports.Add(airport);
                }
            }
            var returnArray = listAirports.Distinct().ToArray();

            if (returnArray is null)
            {
                return NotFound();
            }
            return Ok(returnArray);
        }

        [HttpPost]
        [Route("flights/search")]

        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {
            var matchingList = FlightStorage.AddSearchedFlight(request);

            var page = new PageResult<Flight>()
            {
                Page = matchingList.Length,
                TotalItems = matchingList.Length,
                Items = matchingList
            };

            if (request.To != null && request.From != null && !string.IsNullOrEmpty(request.DepartureDate))
            {
                return Ok(page);
            }

            if (request.To == null || request.From == null || string.IsNullOrEmpty(request.DepartureDate))
            {
                return BadRequest();
            }

            return Ok();
        }


        [HttpGet]
        [Route("flights/{id}")]
        
        public IActionResult FindFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}


