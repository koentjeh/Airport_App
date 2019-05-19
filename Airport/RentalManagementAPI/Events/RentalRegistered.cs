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
        public readonly string StartDate;
        public readonly string EndDate;

        public RentalRegistered(Guid messageId, string rentalId, string renterId, string location, string price, string startDate, string endDate) : base(messageId)
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
