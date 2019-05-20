using Airport.Infrastructure.Messaging;
using Airport.NotificationService.Repositories;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Airport.NotificationService.Events;
using Newtonsoft.Json.Linq;
using Airport.NotificationService.Model;
using Airport.NotificationService.NotificiationChannels;

namespace NotificationService
{
    public class NotificationManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        INotificationRepository _repo;
        IEMailNotifier _emailNotifier;

        public NotificationManager(IMessageHandler messageHandler, INotificationRepository repo, IEMailNotifier emailNotifier)
        {
            _messageHandler = messageHandler;
            _repo = repo;
            _emailNotifier = emailNotifier;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            JObject messageObject = MessageSerializer.Deserialize(message);
            switch (messageType)
            {
                case "CustomerRegistered":
                    await HandleAsync(messageObject.ToObject<CustomerRegistered>());
                    break;
                default:
                    break;
            }
            return true;
        }

        private async Task HandleAsync(CustomerRegistered cr)
        {
            Customer customer = new Customer
            {
                CustomerId = cr.CustomerId,
                Name = cr.Name,
                Address = cr.Address,
                City = cr.City,
                Phone = cr.Phone,
                Luggage = cr.Luggage
            };

            await _repo.RegisterCustomerAsync(customer);
        }
    }
}