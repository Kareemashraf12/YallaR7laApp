using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YallaR7la.Data;
using YallaR7la.Data.Models;
using YallaR7la.DtoModels;

namespace YallaR7la.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration configuration;
        private readonly AppDbContext _appDbContext;
        public UsersController(AppDbContext appDbContext , UserManager<User> userManager,IConfiguration configuration, IMemoryCache memoryCache)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            this.configuration = configuration;
            _memoryCache = memoryCache;
        }




        


        [HttpPost("AddNewUser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddNewUser([FromForm] MdlUser newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           

            // Process the image file
            using var stream = new MemoryStream();
            await newUser.ImageData.CopyToAsync(stream);

           
            var user = new User
            {
                UserName = newUser.Email, 
                Name = newUser.Name,
                Email = newUser.Email,
                PhoneNumper = newUser.PhoneNumper,
                City = newUser.City,
                Prefrance = newUser.Prefrance,
                BirthDate = newUser.BirthDate,
                ImageData = stream.ToArray(),
                UniqueIdImage = Guid.NewGuid()
            };
            
            IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
            if (result.Succeeded)
            {
                return Ok("Success");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return BadRequest(ModelState);


        }



        #region Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(MdlLogin login)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, login.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Email, user.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        claims.Add(new Claim("UserType", "User"));
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken
                        (
                           claims: claims,
                           issuer: configuration["JWT:issuer"],
                           audience: configuration["JWT:Audience"],
                           expires:DateTime.Now.AddHours(1),
                           signingCredentials: sc
                           

                        );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return Ok(_token);

                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    ModelState.AddModelError("", "User Email is not valid!");
                }
            }
            return BadRequest(ModelState);
        }
        #endregion


        //-------------------------------------------------

        //#region Get All Users
        //[HttpGet("GetAllUsers")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _appDbContext.Users.ToListAsync();
        //    return Ok(users);
        //}
        //#endregion


        #region Add User
        /// <summary>
        /// Do not need it now 
        /// </summary>
        /// <param name="mdlUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUser([FromForm] MdlUser mdlUser)
        {
            using var stream = new MemoryStream();
            await mdlUser.ImageData.CopyToAsync(stream);
            var user = new User
            {
                Name = mdlUser.Name,
                Email = mdlUser.Email,

                PhoneNumper = mdlUser.PhoneNumper,
                City = mdlUser.City,
                Prefrance = mdlUser.Prefrance,
                BirthDate = mdlUser.BirthDate,
                ImageData = stream.ToArray(),
                UniqueIdImage = Guid.NewGuid()

            };
            await _appDbContext.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return Ok(user);
        }
        #endregion
        // --------------- feedback methods-----------------


        #region Add Comment
        [HttpPost("AddFeedback/{destinationId}")]
        [Authorize(policy: "UserOnly")]
        public async Task<IActionResult> AddComment(string destinationId ,[FromForm]MdlFeedback mdlFeedback)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("This User can not add comment!");
            }
            var comment = new Feedback()
            {
                Content = mdlFeedback.Content,
                SentimentScore = mdlFeedback.SentimentScore,
                UserId = userId,
                DestinationId = destinationId
            };
            await _appDbContext.AddAsync(comment);
            await _appDbContext.SaveChangesAsync();
            return Ok(comment);
        }
        #endregion


        #region Update Comment

        [HttpPut("UpdateComment/{userId}")]
        public async Task<IActionResult> UpdateComment (string userId, [FromForm] MdlFeedback mdlfeedback)
        {
            var comment = await _appDbContext.Destinations.FindAsync(userId);
            if (comment == null)
            {
                return NotFound("No comment to update!");

            }

            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (mdlfeedback.UserId != user)
            {
                return Forbid("You cant update this comment!");
            }

            var commentToUpdate = new Feedback()
            { 
                Content = mdlfeedback.Content,
                SentimentScore = mdlfeedback.SentimentScore,
            };

            return Ok(commentToUpdate);

        }

        #endregion



        #region Delete Comment

        [HttpDelete("DeleteComment/{feedbackId}")]
        public async Task<IActionResult> DeleteComment(string feedbackId)
        {
            var comment = await _appDbContext.Feedbacks.FindAsync(feedbackId);
            if (comment == null) {
                return NotFound(" No comment to delete !");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (comment.UserId != userId)
            {
                return Forbid("You can not delete This comment!");
            }
            _appDbContext.Feedbacks.Remove(comment);
            await _appDbContext.SaveChangesAsync();
            return Ok("Comment is deleted!");

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


        #region Delete From Favorites

        [HttpDelete("DeleteFromFavorites/{favoriteId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFromFavorites (string favoriteId)
        {
            var favorite = await _appDbContext.Favorites.FindAsync(favoriteId);
            if (favorite == null)
            {
                return NotFound("Destination is not in favorites!");

            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (favorite.UserId !=  currentUserId)
            {
                return Forbid("You do not have Access to remove that from Favorites!");
            }

            _appDbContext.Favorites.Remove(favorite);
            await _appDbContext.SaveChangesAsync();
            return Ok("Destination removed from favorites ");
        }

        #endregion


        [HttpPatch]
        public async Task<IActionResult> ResetPassword(string email, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("Your email is not found. Please check it!");
            }

            // Generate a password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password using the token
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { message = "Password reset failed", errors });
            }

            return Ok("Your password has been updated successfully.");
        }



    }
}
