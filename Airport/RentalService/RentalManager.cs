using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Airport.Infrastructure.Messaging;
using Airport.RentalService.Events;
using Airport.RentalService.Model;
using Airport.RentalService.Repositories;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airport.RentalService
{
    public class RentalManager : IHostedService, IMessageHandlerCallback
    {
        private const decimal HOURLY_RATE = 18.50M;
        private IMessageHandler _messageHandler;
        private IRentalRepository _repo;

        public RentalManager(IMessageHandler messageHandler, IRentalRepository repo)
        {
            _messageHandler = messageHandler;
            _repo = repo;
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
                case "RentalRegistered":
                    await HandleAsync(messageObject.ToObject<RentalRegistered>());
                    break;
            }
            return true;
        }

        private async Task HandleAsync(RentalRegistered fr)
        {
            Rental rental = new Rental
            {
                RentalId = fr.RentalId,
                RenterId = fr.RenterId,
                Location = fr.Location,
                Price = fr.Price,
                StartDate = fr.StartDate,
                EndDate = fr.EndDate
            };

            await _repo.RegisterRentalAsync(rental);
        }
    }
}
