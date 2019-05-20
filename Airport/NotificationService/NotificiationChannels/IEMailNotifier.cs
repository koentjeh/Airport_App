using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Airport.NotificationService.NotificiationChannels
{
    public interface IEMailNotifier
    {
        Task SendEmailAsync(string to, string from, string subject, string body);
    }
}
