using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airport.FlightManagementAPI.Model
{
    public class Flight
    {
        public string FlightId { get; set; }
        public string DepartureDate { get; set; }
        public string Runway { get; set; }
        public string ArrivalDate { get; set; }
        public string City { get; set; }
        public string Pilot { get; set; }

    }
}
