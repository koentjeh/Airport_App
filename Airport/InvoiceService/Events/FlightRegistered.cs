using Airport.Infrastructure.Messaging;
using System;

namespace Airport.InvoiceService.Events
{
    public class FlightRegistered : Event
    {
        public readonly string FlightId;
        public readonly DateTime DepartureDate;
        public readonly string Gate;
        public readonly string CheckInGate;
        public readonly DateTime ArrivalDate;
        public readonly string City;
        public readonly string Pilot;

        public FlightRegistered(Guid messageId, string flightId, DateTime departureDate, string gate, string checkingate, DateTime arrivalDate, string city, string pilot) : base(messageId)
        {
            FlightId = flightId;
            DepartureDate = departureDate;
            Gate = gate;
            CheckInGate = checkingate;
            ArrivalDate = arrivalDate;
            City = city;
            Pilot = pilot;
        }
    }
}
