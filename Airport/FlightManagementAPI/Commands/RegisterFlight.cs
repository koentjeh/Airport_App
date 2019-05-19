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
        public readonly string DepartureDate;
        public readonly string Runway;
        public readonly string ArrivalDate;
        public readonly string City;
        public readonly string Pilot;

        public RegisterFlight(Guid messageId, string flightId, string departureDate , string runway , string arrivalDate , string city , string pilot) : base(messageId)
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
