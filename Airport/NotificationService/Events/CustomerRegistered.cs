using System;
using Airport.Infrastructure.Messaging;
using System.Collections.Generic;
using System.Text;

namespace Airport.NotificationService.Events
{
    public class CustomerRegistered
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool Luggage { get; set; }

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
