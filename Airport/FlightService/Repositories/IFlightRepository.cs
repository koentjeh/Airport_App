using System;
using System.Collections.Generic;
using System.Text;
using Airport.FlightService.Model;
using System.Threading.Tasks;
using System.Transactions;

namespace Airport.FlightService.Repositories
{
    public interface IFlightRepository
    {
        Task RegisterFlightAsync(Flight flight);
        Task<Flight>GetFlightAsync(string flightId);
    }
}
