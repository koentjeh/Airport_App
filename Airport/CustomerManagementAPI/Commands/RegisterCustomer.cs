using System;
using Airport.Infrastructure.Messaging;


namespace Airport.CustomerManagementAPI.Commands
{
    public class RegisterCustomer : Command
    {
        public readonly string CustomerId;
        public readonly string Name;
        public readonly string Address;
        public readonly string City;
        public readonly string Phone;
        public readonly bool Luggage;

        public RegisterCustomer(Guid messageId, string customerId, string name, string address, string city, string phone, bool luggage) : base(messageId)
        {
            CustomerId = customerId;
            Name = name;
            Address = address;
            City = city;
            Phone = phone;
            Luggage = luggage;
        }
    }
}
