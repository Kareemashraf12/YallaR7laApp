using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YallaR7la.Data;
using YallaR7la.Data.Models;
using YallaR7la.DtoModels;

namespace YallaR7la.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationImgController : ControllerBase
    {

        private readonly AppDbContext _appDbContext;
        public DestinationImgController(AppDbContext appDbContext)
        {
            
             _appDbContext = appDbContext;
        }

        #region Get Images By Destination Id
        [HttpGet("GetImagesByDestinationId/{destinationId}")]
        public async Task<IActionResult> GetImagesByDestinationId(string destinationId)
        {
            var images = await _appDbContext.DestinationImages.Where(img => img.DestinationId == destinationId).Select(img => new
            {
                img.ImageId,
                img.DestinationId,
                ImageData = Convert.ToBase64String(img.ImageData),

            }).ToListAsync();

            if (!images.Any())
            {
                return BadRequest("This image not found!");
            }
            return Ok(images);
        }
        #endregion



        //#region Add Images to distanation
        //[HttpPost("AddImagestodistanation")]

        //public async Task<IActionResult> AddDestinationImages([FromForm]MdlDistanationImages mdlDistanationImages)
        //{
        //    using var stream = new MemoryStream();
        //    await mdlDistanationImages.ImageData.CopyToAsync(stream);
        //    var distinationImages = new DestinationImages()
        //    {
        //        ImageData = stream.ToArray(),
        //        UniqeImageId = Guid.NewGuid(),
        //        DestinationId = mdlDistanationImages.DestinationId
        //    };
        //    await _appDbContext.AddAsync(distinationImages);
        //    await _appDbContext.SaveChangesAsync();
        //    return Ok(distinationImages);
        //}
        //#endregion



        #region Get All Comments

        [HttpGet("GetAllComments/{destinationId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllComments (string destinationId)
        {
            var destinationComments = await _appDbContext.Feedbacks.Where(f => f.DestinationId == destinationId).ToListAsync();           
            if (string.IsNullOrWhiteSpace(destinationId) ||  destinationComments.Count == 0)
            {
                return BadRequest("DestinationId is required.");
            }
            

            if (destinationComments == null || destinationComments.Count == 0)
            {
                return NotFound("No comments found for this destination.");
            }
            return Ok(destinationComments);
        }

        #endregion

        #region Add To Favorites

        [HttpPost("AddToFavorites/{destinationId}")]
        [Authorize] // Ensure only authenticated users can access 
        public async Task<IActionResult> AddToFavorites(string destinationId)
        {
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Check if the favorite record already exists to avoid duplicates
            var existingFavorite = await _appDbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.DestinationId == destinationId);
            if (existingFavorite != null)
            {
                return BadRequest("This destination is already in your favorites.");
            }

            
            var favorite = new Favorite
            {
                UserId = userId,
                DestinationId = destinationId,
                
            };

            await _appDbContext.Favorites.AddAsync(favorite);
            await _appDbContext.SaveChangesAsync();

            return Ok(favorite);
        }

        #endregion
    }
}
