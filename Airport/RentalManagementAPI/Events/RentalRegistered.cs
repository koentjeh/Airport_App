using Airport.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.RentalManagementAPI.Events
{
    public class RentalRegistered : Event
    {
        public readonly string RentalId;
        public readonly string RenterId;
        public readonly string Location;
        public readonly string Price;
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;

        public RentalRegistered(Guid messageId, string rentalId, string renterId, string location, string price, DateTime startDate, DateTime endDate) : base(messageId)
        {
            RentalId = rentalId;
            RenterId = renterId;
            Location = location;
            Price = price;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
