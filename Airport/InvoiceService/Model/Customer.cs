using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Airport.InvoiceService.Model
{
   public class Customer
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool Luggage { get; set; }
    }
}
