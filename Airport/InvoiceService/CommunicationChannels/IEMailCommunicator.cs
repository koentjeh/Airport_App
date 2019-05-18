using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Airport.InvoiceService.CommunicationChannels
{
    public interface IEMailCommunicator
    {
        Task SendEmailAsync(MailMessage message);
    }
}
