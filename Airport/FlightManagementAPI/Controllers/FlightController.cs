using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Airport.FlightManagementAPI.DataAccess;
using Airport.FlightManagementAPI.Model;
using AutoMapper;
using Airport.Infrastructure.Messaging;
using Airport.FlightManagementAPI.Events;
using Airport.FlightManagementAPI.Commands;

namespace Airport.FlightManagementAPI.Cotrollers
{
    [Route("/api/[controller]")]
    public class FlightController : Controller
    {
        IMessagePublisher _messagePublisher;
        FlightManagementDBContext _dbContext;

        public FlightController(FlightManagementDBContext dbContext, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _dbContext.Flights.ToListAsync());
        }

        [HttpGet]
        [Route("{flightId}", Name = "GetByFlightId")]
        public async Task<IActionResult> GetByFlightId(string flightId)
        {
            var flight = await _dbContext.Flights.FirstOrDefaultAsync(f => f.FlightId == flightId);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterFlight command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // insert flight
                    Flight flight = Mapper.Map<Flight>(command);
                    _dbContext.Flights.Add(flight);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    FlightRegistered e = Mapper.Map<FlightRegistered>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                    // return result
                    return CreatedAtRoute("GetByFlightId", new { flightId = flight.FlightId }, flight);
                }
                return BadRequest();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
