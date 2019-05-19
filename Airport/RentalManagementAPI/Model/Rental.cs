﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.RentalManagementAPI.Model
{
    public class Rental
    {
        public string RentalId { get; set; }
        public string RenterId { get; set; }
        public string Location { get; set; }
        public string Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
