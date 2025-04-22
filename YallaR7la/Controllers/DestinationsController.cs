using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YallaR7la.Data;
using YallaR7la.Data.Models;
using YallaR7la.DtoModels;

namespace YallaR7la.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DestinationsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public DestinationsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Get All Destinations 
        [HttpGet]
        public async Task<IActionResult> GetAllDestinations()
        {
            var destinations = await _appDbContext.Destinations.Include(d => d.destinationImages).OrderBy(c=> c.AverageRating).ToListAsync();
            return Ok(destinations);
        }
        #endregion


        #region Add Destination
        [HttpPost]
        public async Task<IActionResult> AddDestination(MdlDestination mdlDestination)
        {
            var destination = new Destination()
            {
                Name = mdlDestination.Name,
                Description = mdlDestination.Description,
                Location = mdlDestination.Location,
                Category = mdlDestination.Category,
                AvilableNumber = mdlDestination.AvilableNumber,
                Rating = mdlDestination.Rating,
                
                StartDate = mdlDestination.StartDate,
                EndtDate = mdlDestination.EndtDate,
                IsAvelable = mdlDestination.IsAvelable,
                Discount = mdlDestination.Discount,
                Cost = mdlDestination.Cost - ((mdlDestination.Discount * 100 ) * mdlDestination.Cost),
                BusinessOwnerId = mdlDestination.BusinessOwnerId
            };
            await _appDbContext.AddAsync(destination);
            await _appDbContext.SaveChangesAsync();
            return Ok(destination);
        }
        #endregion



        #region Book Destination

        [HttpPut("Booking/{destinationId}")]
        public async Task<IActionResult> BookDestination (string destinationId)
        {
            var destination = await _appDbContext.Destinations.FindAsync(destinationId);
            if (destination == null)
            {
                return NotFound("Destination not found.");
            }
            if (destination.AvilableNumber <=  0)
            {
                return BadRequest("Sorry, the destination is fully booked.");
            }
            destination.AvilableNumber--;
            await _appDbContext.SaveChangesAsync();
            return Ok(destination);
        }


        #endregion


        #region UnBook Destination

        [HttpPut("UnBookDestination/{destinationId}")]
        public async Task<IActionResult> UnBookDestination(string destinationId)
        {
            var destination = await _appDbContext.Destinations.FindAsync(destinationId);
            if (destination == null)
            {
                return NotFound("Destination not found.");
            }
            
            destination.AvilableNumber++;
            await _appDbContext.SaveChangesAsync();
            return Ok(destination);
        }


        #endregion



    }
}
