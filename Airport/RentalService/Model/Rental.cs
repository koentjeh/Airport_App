using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.RentalService.Model
{
    public class Rental
    {
        public string RentalId { get; set; }
        public string RenterId { get; set; }
        public string Location { get; set; }
        public string Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
