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
    public class favouritesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public favouritesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        #region Get Favorites
        [HttpGet("GetFavorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var favorites = await _appDbContext.Favorites.ToListAsync();
            return Ok(favorites);
        }
        [HttpPost]
        public async Task<IActionResult> AddToFavorite(MdlFavorite mdlFavorite)
        {
            var favorite = new Favorite()
            {
                FavoritedAt = DateTime.Now,
                UserId = mdlFavorite.UserId,
                DestinationId = mdlFavorite.DestinationId
            };
            await _appDbContext.AddAsync(favorite);
            await _appDbContext.SaveChangesAsync();
            return Ok(favorite);
        }
        #endregion
    }
}
