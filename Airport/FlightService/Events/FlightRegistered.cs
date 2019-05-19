using Airport.Infrastructure.Messaging;
using System;

namespace Airport.FlightService.Events
{
    public class FlightRegistered : Event
    {
        public readonly string FlightId;
        public readonly DateTime DepartureDate;
        public readonly string Runway;
        public readonly DateTime ArrivalDate;
        public readonly string City;
        public readonly string Pilot;

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
