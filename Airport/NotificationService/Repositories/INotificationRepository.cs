using Airport.NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Airport.NotificationService.Repositories
{
    public interface INotificationRepository
    {
        Task RegisterCustomerAsync(Customer customer);
    }
}
