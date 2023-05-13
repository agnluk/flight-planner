using System.Data.Common;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class SearchFlightsRequest
    {
        public int Id { get; set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        public string DepartureDate { get; set; }
    }
}