using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Airport.RentalManagementAPI.Model;
using Airport.RentalManagementAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Airport.Infrastructure.Messaging;
using Airport.RentalManagementAPI.Events;
using Airport.RentalManagementAPI.Commands;

namespace Airport.RentalManagementAPI.Controllers
{
    [Route("/api/[controller]")]
    public class RentalsController : Controller
    {
        IMessagePublisher _messagePublisher;
        RentalManagementDBContext _dbContext;

        public RentalsController(RentalManagementDBContext dbContext, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _dbContext.Rentals.ToListAsync());
        }

        [HttpGet]
        [Route("{rentalId}", Name = "GetByRentalId")]
        public async Task<IActionResult> GetByRentalId(string rentalId)
        {
            var rental = await _dbContext.Rentals.FirstOrDefaultAsync(v => v.RentalId == rentalId);
            if (rental == null)
            {
                return NotFound();
            }
            return Ok(rental);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRental command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // insert rental
                    Rental rental = Mapper.Map<Rental>(command);
                    _dbContext.Rentals.Add(rental);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<RentalRegistered>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                    //return result
                    return CreatedAtRoute("GetByRentalId", new { rentalId = rental.RentalId }, rental);
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
