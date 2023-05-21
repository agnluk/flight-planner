using FlightPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 1;

        public static Flight GetFlight(int id)
        {
            var flight = _flights.FirstOrDefault(f => f.Id == id);
            return flight;
        }

        public static List<Flight> FlightsClear()
        {
            _flights.Clear();
            _id = 1;

            return _flights;
        }

        public static void UpdateFlights(List<Flight> flights)
        {
            _flights = flights;
        }

        public static Flight AddFlight(Flight addFlight)
        {
            addFlight.Id = _id++;
            _flights.Add(addFlight);

            return addFlight;
        }

        public static List<Flight> GetAllFlights()
        {
            return _flights;
        }

        public static Flight DeleteFlight(int removeFlight)
        {
            var removeById = GetFlight(removeFlight);
            _flights.Remove(removeById);
            return removeById;
        }

        public static List<Airport> GetAirports()
        {
            var listAirports = new List<Airport>();

            foreach (var flight in _flights)
            {
                listAirports.Add(new Airport()
                {
                    AirportCode = flight.From.AirportCode,
                    City = flight.From.City,
                    Country = flight.From.Country,
                });
            }

            var distinctAirports = listAirports.Distinct().ToList();

            return distinctAirports;
        }

        public static bool IsValidFlight(Flight flight)
        {
            return flight == null ||
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
                string.Equals(flight.To.AirportCode, flight.From.AirportCode, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool hasConflict(Flight flight)
        {
            return _flights.Any(f => f.From.Country.Equals(flight.From.Country) &&
                                               f.To.Country.Equals(flight.To.Country) &&
                                               f.From.AirportCode.Equals(flight.From.AirportCode) &&
                                               f.To.AirportCode.Equals(flight.To.AirportCode) &&
                                               f.From.City.Equals(flight.From.City) &&
                                               f.To.City.Equals(flight.To.City) &&
                                               f.ArrivalTime.Equals(flight.ArrivalTime) &&
                                               f.DepartureTime.Equals(flight.DepartureTime) &&
                                               f.Carrier.Equals(flight.Carrier));
        }
    }
}
