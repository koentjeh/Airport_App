using System;
using Airport.InvoiceService.Model;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Airport.InvoiceService.Repositories
{
    public interface IInvoiceRepository
    {
        Task RegisterCustomerAsync(Customer customer);
        Task<Customer> GetCustomerAsync(string customerId);
        Task RegisterFlightAsync(Flight flight);
        Task<Flight> GetFlightAsync(string flightId);
        Task RegisterInvoiceAsync(Invoice invoice);
    }
}
