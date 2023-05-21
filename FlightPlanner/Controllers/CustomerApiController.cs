using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PageResult = FlightPlanner.Models.PageResult;

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
        public IActionResult SearchFlights(SearchFlightsRequest request)
        {
            var matchingList = FlightStorage.GetAllFlights();
            var page = new PageResult();

            if (request.From == null
                || request.To == null
                || request.DepartureDate == null
                || request.To == request.From)
            {
                return BadRequest();
            }

            var uniqueItem = matchingList.Select(item =>
                                           item.From.AirportCode == request.From &&
                                           item.To.AirportCode == request.To &&
                                           item.DepartureTime == request.DepartureDate).Distinct();

            var items = matchingList.Where(item =>
                                          item.From.AirportCode == request.From &&
                                          item.To.AirportCode == request.To &&
                                          item.DepartureTime == request.DepartureDate).Distinct();
            page = new PageResult()
            {
                Page = 0,
                TotalItems = uniqueItem.Count(),
                Items = items.ToArray(),
            };

            if (request.To != null && request.From != null && !string.IsNullOrEmpty(request.DepartureDate))
            {
                return Ok(page);
            }

            return NoContent();
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