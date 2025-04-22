using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Authorize(policy: "OwnerOnly")]
    public class OwnersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<BusinessOwner> ownerManager;
        private readonly IConfiguration configuration;

        public OwnersController(AppDbContext appDbContext ,UserManager<BusinessOwner> ownerManager, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            this.ownerManager = ownerManager;
            this.configuration = configuration;
        }

        // handel destination from owner controller

        #region Get All Destinations 
        [HttpGet]
        public async Task<IActionResult> GetAllDestinations()
        {
            var destinations = await _appDbContext.Destinations.Include(d => d.destinationImages).ToListAsync();
            return Ok(destinations);
        }
        #endregion


        
        #region Get Destination By Id & if id = null get all Dest
        [HttpGet("GetDestinationById ")]
        public async Task<IActionResult> GetDestination([FromQuery]int? destinationId)
        {
            
            if (destinationId == null)
            {
                var destinations = await _appDbContext.Destinations.Include(d => d.destinationImages).ToListAsync();
                return Ok(destinations);
            }
            var destinationById = await _appDbContext.Destinations.FindAsync(destinationId);
            if (destinationById == null)
            {
                return NotFound("The destination is not found!");
            }
            return Ok(destinationById);

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
                Cost = mdlDestination.Cost - ((mdlDestination.Discount * 100) * mdlDestination.Cost),
                BusinessOwnerId = mdlDestination.BusinessOwnerId
            };
            await _appDbContext.AddAsync(destination);
            await _appDbContext.SaveChangesAsync();
            return Ok(destination);
        }
        #endregion


        #region Add Destination Image

        [HttpPost("AddDestinationImage/{destinationId}/{ownerId}")]
        [Authorize(policy: "OwnerOnly")]
        public async Task<IActionResult> AddDestinationImage (string ownerId,string destinationId,MdlDistanationImages mdlDistanationImages)
        {
            var currentOwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ownerId != currentOwnerId )
            {
                return Forbid(" You have not access to do that!");
            }
            if (mdlDistanationImages.ImageData == null || mdlDistanationImages.ImageData.Length == 0)
            {
                return BadRequest("No images uploaded.");
            }
            var destinationImages = new List<DestinationImages>();

            foreach (var file in mdlDistanationImages.ImageData)
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var destinationImage = new DestinationImages
                {
                    ImageData = stream.ToArray(),
                    DestinationId = destinationId
                };
                destinationImages.Add(destinationImage);
                await _appDbContext.AddAsync(destinationImage);
            }

            await _appDbContext.SaveChangesAsync();
            return Ok(destinationImages);
        }

        #endregion



        #region Update Destination Image

        [HttpPut("UpdateDestinationImage/{ownerId}/{destinationId}")]
        public async Task<IActionResult> UpdateDestinationImage(string ownerId, string destinationId, [FromForm] MdlDistanationImages mdlDistanationImages)
        {
            var currentOwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ownerId != currentOwnerId)
            {
                return Forbid("You do not have access to update this destination's images.");
            }
            // Check that images were provided in the request
            if (mdlDistanationImages.ImageData == null || mdlDistanationImages.ImageData.Length == 0)
            {
                return BadRequest("No images uploaded.");
            }

            // Retrieve existing images for the destination
            var existingImages = await _appDbContext.DestinationImages
                .Where(di => di.DestinationId == destinationId)
                .ToListAsync();

            // Remove the existing images
            if (existingImages.Any())
            {
                _appDbContext.DestinationImages.RemoveRange(existingImages);
            }

            // Process and add the new images
            var newImages = new List<DestinationImages>();
            foreach (var file in mdlDistanationImages.ImageData)
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var destinationImage = new DestinationImages
                {
                    ImageData = stream.ToArray(),
                    DestinationId = destinationId
                };
                newImages.Add(destinationImage);
                await _appDbContext.DestinationImages.AddAsync(destinationImage);
            }

            await _appDbContext.SaveChangesAsync();
            return Ok(newImages);
        }

        #endregion



        #region Delete all Destination Image

        [HttpDelete("DeleteAllDestinationImage/{ownerId}/{destintaionId}")]
        public async Task<IActionResult> DeleteAllDestinationImage(string destinationId, string ownerId)
        {
            var currentOwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentOwnerId == ownerId)
            {
                return Forbid("You are not authorized to delete images for this destination.");
            }

            var destination = await _appDbContext.Destinations
        .Include(d => d.destinationImages)
        .FirstOrDefaultAsync(d => d.DestinationId.ToString() == destinationId);
            if (destination == null)
            {
                return NotFound("No destination found .");
            }
            if (destination.destinationImages == null || !destination.destinationImages.Any())
            {
                return NotFound("No images found for this destination.");
            }
            _appDbContext.DestinationImages.RemoveRange(destination.destinationImages);
            await _appDbContext.SaveChangesAsync();
            return Ok("All images for the destination have been deleted.");
        }

        #endregion



        #region Delete One Image

        [HttpDelete("DeleteOneImage/{ownerId}/{destinationId}/{imageId}")]
        public async Task<IActionResult> DeleteOneImage (string destinationId , string ownerId ,string imageId)
        {
            var currentOwnerId = User.FindFirstValue (ClaimTypes.NameIdentifier);
            if (currentOwnerId != ownerId)
            {
                return Forbid("You are not authorized to delete images for this destination.");
            }

            var image = await _appDbContext.DestinationImages.FirstOrDefaultAsync(di => di.DestinationId == destinationId && di.ImageId.ToString()== imageId);

            if (image == null)
            {
                return NotFound("Image not found.");
            }
            _appDbContext.DestinationImages.Remove(image);
            await _appDbContext.SaveChangesAsync();
            return Ok("The image has been deleted.");
        }


        #endregion


        #region Update Destination
        [HttpPut("UpdateDestination/{destinationId}")]
        public async Task<IActionResult> UpdateDestination(string destinationId, [FromForm] MdlDestination mdlDestination)
        {
            // Retrieve the destination to update
            var destination = await _appDbContext.Destinations.FindAsync(destinationId);
            if (destination == null)
            {
                return NotFound("Destination not found.");
            }
            
            
            var currentOwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (destination.BusinessOwnerId != currentOwnerId)
            {
                return Forbid("You are not authorized to update this destination.");
            }

            var destinationToUpdate = new Destination()
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
                Cost = mdlDestination.Cost - ((mdlDestination.Discount * 100) * mdlDestination.Cost),
            };
           

            _appDbContext.Destinations.Update(destinationToUpdate);
            await _appDbContext.SaveChangesAsync();
            return Ok(destination);
        }

        #endregion


        #region Delete Destination

        [HttpDelete("DeleteDestination/{destinationId}")]
        public async Task<IActionResult> DeleteDestination(string destinationId)
        {
            var destination = await _appDbContext.Destinations.FindAsync(destinationId);
            if (destination == null)
            {
                return NotFound("The Destination is not found!");
            }
             
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (destination.BusinessOwnerId != ownerId)
            {
                return Forbid("You can not delete this destination!");
            }
            _appDbContext.Destinations.Remove(destination);
            await _appDbContext.SaveChangesAsync();
            return Ok("The destination is removed!");
        }


        #endregion

        // ------------------------- login -----------------------------
        #region Login As Owner
        [HttpPost("Login")]
        public async Task<IActionResult> Login(MdlLogin login)
        {
            if (ModelState.IsValid)
            {
                BusinessOwner owner = await ownerManager.FindByEmailAsync(login.Email);
                if (owner != null)
                {
                    if (await ownerManager.CheckPasswordAsync(owner, login.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Email, owner.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, owner.OwnerId.ToString()));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        claims.Add(new Claim("UserType", "Owner"));
                        var roles = await ownerManager.GetRolesAsync(owner);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken
                        (
                           claims: claims,
                           issuer: configuration["JWT:issuer"],
                           audience: configuration["JWT:Audience"],
                           expires: DateTime.Now.AddHours(1),
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
                    ModelState.AddModelError("", "Owner Email is not valid!");
                }
            }
            return BadRequest(ModelState);
        }
        #endregion
        //-------------------------------------------------

    }
}
