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
    public class favouritesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public favouritesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        #region MyFavorites

        [HttpGet("MyFavorites")]
        public async Task<IActionResult> GetFavoriteDestinations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found in token.");

            var favorites = await _appDbContext.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Destination).ThenInclude(i => i.destinationImages)
                .Select(f => new
                {
                    f.Destination.DestinationId,
                    f.Destination.Name,
                    f.Destination.Category,
                    f.Destination.Description,
                    f.Destination.AverageRating,
                    f.FavoritedAt,
                    Images = f.Destination.destinationImages.Select(img => new
                    {
                        img.ImageId,
                        ImageBase64 = Convert.ToBase64String(img.ImageData)
                    }).ToList()
                })
                .ToListAsync();

            return Ok(favorites);
        }


        #endregion

        #region Add To Favorite
        [HttpPost("AddToFavorite")]
        public async Task<IActionResult> AddToFavorite([FromBody] MdlFavorite mdlFavorite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found in token.");

            // Check if already favorited
            bool exists = await _appDbContext.Favorites
                .AnyAsync(f => f.UserId == userId && f.DestinationId == mdlFavorite.DestinationId);

            if (exists)
                return BadRequest("Destination is already in favorites.");

            var favorite = new Favorite
            {
                UserId = userId,
                DestinationId = mdlFavorite.DestinationId,
                FavoritedAt = DateTime.UtcNow
            };

            await _appDbContext.Favorites.AddAsync(favorite);
            await _appDbContext.SaveChangesAsync();

            return Ok(new { message = "Added to favorites." });
        }

        #endregion


        #region Remove from Favorites

        [HttpDelete("RemoveFromFavorite/{destinationId}")]
        public async Task<IActionResult> RemoveFromFavorite(string destinationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found in token.");

            var favorite = await _appDbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.DestinationId == destinationId);

            if (favorite == null)
                return NotFound("Destination not found in your favorites.");

            _appDbContext.Favorites.Remove(favorite);
            await _appDbContext.SaveChangesAsync();

            return Ok(new { message = "Removed from favorites." });
        }


        #endregion
    }
}
