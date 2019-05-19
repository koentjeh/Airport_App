using System;
using Airport.RentalService.Model;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Airport.RentalService.Repositories
{
    public interface IRentalRepository
    {
        Task RegisterRentalAsync(Rental rental);
        Task<Rental> GetRentalAsync(string rentalId);
    }
}
