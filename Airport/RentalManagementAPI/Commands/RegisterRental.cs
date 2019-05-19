using System;
using Airport.Infrastructure.Messaging;


namespace Airport.RentalManagementAPI.Commands
{
    public class RegisterRental : Command
    {
        public readonly string RentalId;
        public readonly string RenterId;
        public readonly string Location;
        public readonly string Price;
        public readonly string StartDate;
        public readonly string EndDate;

        public RegisterRental(Guid messageId, string rentalId, string renterId, string location, string price, string startDate, string endDate) : base(messageId)
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
