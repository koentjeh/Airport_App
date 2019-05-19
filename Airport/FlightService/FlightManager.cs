using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Airport.Infrastructure.Messaging;
using Airport.FlightService.Events;
using Airport.FlightService.Model;
using Airport.FlightService.Repositories;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airport.FlightService
{
    public class FlightManager : IHostedService, IMessageHandlerCallback
    {
        private const decimal HOURLY_RATE = 18.50M;
        private IMessageHandler _messageHandler;
        private IFlightRepository _repo;

        public FlightManager(IMessageHandler messageHandler, IFlightRepository repo)
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
                case "FlightRegistered":
                    await HandleAsync(messageObject.ToObject<FlightRegistered>());
                    break;
            }
            return true;
        }

        private async Task HandleAsync(FlightRegistered fr)
        {
            Flight flight = new Flight
            {
                FlightId = fr.FlightId,
                DepartureDate = fr.DepartureDate.ToString(),
                Runway = fr.Runway,
                ArrivalDate = fr.ArrivalDate.ToString(),
                City = fr.City,
                Pilot = fr.Pilot
            };

            await _repo.RegisterFlightAsync(flight);
        }
    }
}
