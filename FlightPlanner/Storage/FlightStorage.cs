using FlightPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Storage
{
    public static  class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 1;

        public static Flight GetFlight(int id)
        {
           return _flights.SingleOrDefault(f => f.Id == id);
        }
        public static List<Flight> FlightsClear()
        {
            _flights.Clear();
            return _flights;
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

            return listAirports;
        }


    }
}
