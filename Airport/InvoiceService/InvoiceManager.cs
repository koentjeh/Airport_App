using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Airport.Infrastructure.Messaging;
using Airport.InvoiceService.CommunicationChannels;
using Airport.InvoiceService.Events;
using Airport.InvoiceService.Model;
using Airport.InvoiceService.Repositories;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airport.InvoiceService
{
    public class InvoiceManager : IHostedService, IMessageHandlerCallback
    {
        private const decimal HOURLY_RATE = 18.50M;
        private IMessageHandler _messageHandler;
        private IInvoiceRepository _repo;
        private IEMailCommunicator _emailCommunicator;

        public InvoiceManager(IMessageHandler messageHandler, IInvoiceRepository repo, IEMailCommunicator emailCommunicator)
        {
            _messageHandler = messageHandler;
            _repo = repo;
            _emailCommunicator = emailCommunicator;
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
                case "FlightRegistered":
                    await HandleAsync(messageObject.ToObject<FlightRegistered>());
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

        private async Task HandleAsync(FlightRegistered fr)
        {
            Flight flight = new Flight
            {
                FlightId = fr.FlightId,
                DepartureDate = fr.DepartureDate,
                Gate = fr.Gate,
                CheckInGate = fr.CheckInGate,
                ArrivalDate = fr.ArrivalDate,
                City = fr.City,
                Pilot = fr.Pilot
            };

            await _repo.RegisterFlightAsync(flight);
        }

        private async Task SendInvoice(Customer customer, Flight flight, Invoice invoice)
        {
            StringBuilder body = new StringBuilder();

            // top banner
            body.AppendLine("<htm><body style='width: 1150px; font-family: Arial;'>");

            body.AppendLine("<table style='width: 100%; border: 0px; font-size: 25pt;'><tr>");
            body.AppendLine("<td>Airport</td>");
            body.AppendLine("<td style='text-align: right;'>INVOICE</td>");
            body.AppendLine("</tr></table>");

            body.AppendLine("<hr>");

            // invoice and customer details
            body.AppendLine("<table style='width: 100%; border: 0px;'><tr>");

            body.AppendLine("<td width='150px' valign='top'>");
            body.AppendLine("Invoice reference<br/>");
            body.AppendLine("Invoice date<br/>");
            body.AppendLine("Amount<br/>");
            body.AppendLine("Payment due by<br/>");
            body.AppendLine("</td>");

            body.AppendLine("<td valign='top'>");
            body.AppendLine($": {invoice.InvoiceId}<br/>");
            body.AppendLine($": {invoice.InvoiceDate.ToString("dd-MM-yyyy")}<br/>");
            body.AppendLine($": &euro; {invoice.Amount}<br/>");
            body.AppendLine($": {invoice.InvoiceDate.AddDays(30).ToString("dd-MM-yyyy")}<br/>");
            body.AppendLine("</td>");

            body.AppendLine("<td width='50px' valign='top'>");
            body.AppendLine("To:");
            body.AppendLine("</td>");

            body.AppendLine("<td valign='top'>");
            body.AppendLine($"{customer.Name}<br/>");
            body.AppendLine($"{customer.Address}<br/>");
            body.AppendLine($"{customer.City}<br/>");
            body.AppendLine($"{customer.Phone}<br/>");
            body.AppendLine($"{customer.Luggage}<br/>");
            body.AppendLine("</td>");

            body.AppendLine("<td width='50px' valign='top'>");
            body.AppendLine("Flight details:");
            body.AppendLine("</td>");

            body.AppendLine("<td valign='top'>");
            body.AppendLine($"{flight.Gate}<br/>");
            body.AppendLine($"{flight.CheckInGate}<br/>");
            body.AppendLine($"{flight.DepartureDate}<br/>");
            body.AppendLine($"{flight.ArrivalDate}<br/>");
            body.AppendLine($"{flight.City}<br/>");
            body.AppendLine("</td>");

            body.AppendLine("</tr></table>");

            body.AppendLine("<hr><br/>");

            // body
            body.AppendLine($"Dear {customer.Name},<br/><br/>");
            body.AppendLine("Hereby we send you an invoice for your ticket you bought on the Airport:<br/>");

            body.AppendLine("<ol>");
            foreach (string specificationLine in invoice.Specification.Split('\n'))
            {
                if (specificationLine.Length > 0)
                {
                    body.AppendLine($"<li>{specificationLine}</li>");
                }
            }
            body.AppendLine("</ol>");


            body.AppendLine($"Total amount : &euro; {invoice.Amount}<br/><br/>");

            body.AppendLine("Payment terms : Payment within 30 days of invoice date.<br/><br/>");

            // payment details
            body.AppendLine("Payment details<br/><br/>");

            body.AppendLine("<table style='width: 100%; border: 0px;'><tr>");

            body.AppendLine("<td width='120px' valign='top'>");
            body.AppendLine("Bank<br/>");
            body.AppendLine("Name<br/>");
            body.AppendLine("IBAN<br/>");
            body.AppendLine($"Reference<br/>");
            body.AppendLine("</td>");

            body.AppendLine("<td valign='top'>");
            body.AppendLine(": ING<br/>");
            body.AppendLine(": Airport<br/>");
            body.AppendLine(": NL20INGB0001234567<br/>");
            body.AppendLine($": {invoice.InvoiceId}<br/>");
            body.AppendLine("</td>");

            body.AppendLine("</tr></table><br/>");

            // greetings
            body.AppendLine("Greetings,<br/><br/>");
            body.AppendLine("The Airport crew<br/>");

            body.AppendLine("</htm></body>");

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("invoicing@airport.nl"),
                Subject = $"Airport Invoice #{invoice.InvoiceId}"
            };
            mailMessage.To.Add("airport@prestoprint.nl");

            mailMessage.Body = body.ToString();
            mailMessage.IsBodyHtml = true;

            await _emailCommunicator.SendEmailAsync(mailMessage);
        }
    }
}
