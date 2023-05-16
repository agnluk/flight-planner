using System.Data.Common;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class SearchFlightsRequest
    {
        public Airport From { get; set; }
        public Airport To { get; set; }
        public string DepartureDate { get; set; }
    }
}