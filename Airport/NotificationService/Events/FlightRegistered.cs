using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.NotificationService.Events
{
    public class FlightRegistered
    {
        public string FlightId { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Runway { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string City { get; set; }
        public string Pilot { get; set; }

        public FlightRegistered(Guid messageId, string flightId, DateTime departureDate, string runway, DateTime arrivalDate, string city, string pilot) : base(messageId)
        {
            FlightId = flightId;
            DepartureDate = departureDate;
            Runway = runway;
            ArrivalDate = arrivalDate;
            City = city;
            Pilot = pilot;
        }
    }
}
