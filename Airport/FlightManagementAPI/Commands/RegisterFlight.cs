using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airport.Infrastructure.Messaging;

namespace Airport.FlightManagementAPI.Commands
{
    public class RegisterFlight : Command
    {
        public readonly string FlightId;
        public readonly DateTime DepartureDate;
        public readonly string Gate;
        public readonly string CheckInCounter;
        public readonly DateTime ArrivalDate;
        public readonly string City;
        public readonly string Pilot;

        public RegisterFlight(Guid messageId, string flightId, DateTime departureDate, string gate, string checkInCounter, DateTime arrivalDate , string city , string pilot) : base(messageId)
        {
            FlightId = flightId;
            DepartureDate = departureDate;
            Gate = gate;
            CheckInCounter = checkInCounter;
            ArrivalDate = arrivalDate;
            City = city;
            Pilot = pilot;
        }
    }
}
