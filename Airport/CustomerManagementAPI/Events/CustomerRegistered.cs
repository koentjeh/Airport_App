using Airport.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.CustomerManagementAPI.Events
{
    public class CustomerRegistered : Event
    {
        public readonly string CustomerId;
        public readonly string Name;
        public readonly string Address;
        public readonly string City;
        public readonly string Phone;
        public readonly bool Luggage;

        public CustomerRegistered(Guid messageId, string customerId, string name, string address, string city, string phone, bool luggage) : base(messageId)
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
