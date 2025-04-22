using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    //[Authorize(Policy = "AdminOnly")]
    public class AdminsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<Admin> _adminManager;
        private readonly UserManager<BusinessOwner> _ownerManager;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public AdminsController(AppDbContext appDbContext , UserManager<Admin> adminManager , UserManager<BusinessOwner> ownerManager, IConfiguration configuration , IMemoryCache memoryCache)
        {
            _appDbContext = appDbContext;
            _adminManager = adminManager;
            _ownerManager = ownerManager;
            _configuration = configuration;
            _memoryCache = memoryCache;
            
        }

        [HttpGet]

        //public  async Task<IActionResult> GetAllAdmins()
        //{
        //    var allAdmins = await _appDbContext.Admins.ToListAsync();
        //    return Ok(allAdmins);
        //}

        // Get admin with Id 
        //[HttpGet("GetAdminDetails/{adminId}")]
        //public async Task<IActionResult> GetAdminDetails(int adminId)
        //{
        //    var adminDetails = await _appDbContext.Admins
        //        .Where(a => a.AdminId == adminId)
        //        .Select(a => new AdminDto
        //        {
        //            Name = a.Name,
        //            Email = a.Email,
        //            PhoneNumper = a.PhoneNumper,
        //            CreatedAt = a.CreatedAt
        //        })
        //        .FirstOrDefaultAsync();

        //    if (adminDetails == null)
        //    {
        //        return NotFound(new { Message = "Admin not found" });
        //    }

        //    return Ok(adminDetails);
        //}
        // --------------

        #region Get All Admin
        [HttpGet("GetAllAdmin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var allAdmin = await _appDbContext.Admins
                .Select(a => new AdminDto
                {
                    Name = a.Name,
                    Email = a.Email,
                    PhoneNumper = a.PhoneNumper,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            if (allAdmin == null)
            {
                return NotFound(new { Message = "Admin not found" });
            }

            return Ok(allAdmin);
        }
        #endregion
        //-------------------

        #region Get Admin Details
        [HttpGet("GetAdminDetails/{adminId}")]
        public async Task<IActionResult> GetAdminDetails(string adminId)
        {
            var adminDetails = await _appDbContext.Admins.Where(a => a.AdminId == adminId).SingleOrDefaultAsync();
            if (adminDetails == null) {
                return NotFound(new { Message = "This Admin is not foud!" });
                    }
            return Ok(adminDetails);
        }
#endregion


        #region Add Admin 
        [HttpPost("AddAdmin")]
        [AllowAnonymous]

        public async Task<IActionResult> AddAdmin(MdlAdmin mdlAdmin)
        {
            //var currrentadmin = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (currrentadmin == null || currrentadmin != adminId)
            //{
            //    return Forbid("You do not have access to add admin!");
            //}

            
            using var stream = new MemoryStream();
            await mdlAdmin.ImageData.CopyToAsync(stream);
            var admin = new Admin()
            {
                Name = mdlAdmin.Name,
                Email = mdlAdmin.Email,
                Password = mdlAdmin.Password,
                PhoneNumper = mdlAdmin.PhoneNumper,
                
                ImageData = stream.ToArray(),
                UniqeImageId = Guid.NewGuid(),
            };

            await _appDbContext.AddAsync(admin);
            await _appDbContext.SaveChangesAsync();
            return Ok(admin);

        }

        #endregion


        #region  Forget Password

        [HttpPut]

        public async Task<IActionResult> FoegetPassword(PasswordDto passwordDto)
        {
            var adminEmail = await _appDbContext.Admins.FindAsync(passwordDto.Email);
            if (adminEmail == null)
            {
                return NotFound("Your Email is not found try again!");
            }

            var newAdminpassword = new Admin()
            {
                Password = passwordDto.Password
            };
            return Ok("your password updated!");

        }
        


        

        #endregion


        #region Update Admin Info
        [HttpPut("Update_Admin_Info/{adminId}")]
        public async Task<IActionResult> UpdateAdminInfo(string adminId, [FromForm] MdlAdmin mdlAdmin)
        {
            var admin = await _appDbContext.Admins.FindAsync(adminId);
            var oldImage = admin.ImageData;
            if (admin == null) { return NotFound("this admin not found!"); }
            //var existingAdminData = new
            //{
            //    admin.Name,
            //    admin.Email,
            //    admin.Password,
            //    admin.ImageData,
            //    admin.PhoneNumper

            //};
            var adminDetails = await _appDbContext.Admins.Where(a => a.AdminId == adminId).SingleOrDefaultAsync();
            if (adminDetails == null)
            {
                return NotFound(new { Message = "This Admin is not foud!" });
            }
            else
                return Ok(adminDetails);

            if (mdlAdmin.ImageData != null)
            {
                using var stream = new MemoryStream();
                await mdlAdmin.ImageData.CopyToAsync(stream);
                admin.ImageData = stream.ToArray();
            }
            else
            {
                admin.ImageData = oldImage ;
            }
            admin.Name = mdlAdmin.Name;
            admin.Email = mdlAdmin.Email;
            admin.Password = mdlAdmin.Password;
            admin.UpdatedAt = DateTime.Now;
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Admin data updated successfully!",
                //OldData = existingAdminData,
                UpdatedData = new
                {
                    admin.AdminId,
                    admin.Name,
                    admin.Email,
                    admin.UpdatedAt
                }
            });
        }

        #endregion


        //----------------------------------
        #region Update Admin Info3
        [HttpPut("Update_Admin_Info3/{adminId}")]
        public async Task<IActionResult> UpdateAdminInfo3(string adminId, [FromForm] MdlAdmin mdlAdmin)
        {
            //  Retrieve admin data from the database
            var admin = await _appDbContext.Admins.FindAsync(adminId);
            if (admin == null)
            {
                return NotFound(new { Message = "This admin was not found!" });
            }

            //  Send existing data before updating
            var existingData = new
            {
                admin.AdminId,
                admin.Name,
                admin.Email,
                admin.UpdatedAt
            };

            //  If no new data is provided, return the existing data
            if (mdlAdmin.Name == null && mdlAdmin.Email == null && mdlAdmin.Password == null && mdlAdmin.ImageData == null)
            {
                return Ok(new
                {
                    Message = "Current admin data retrieved successfully!",
                    ExistingData = existingData
                });
            }

            
            byte[] oldImageData = admin.ImageData;

            
            admin.Name = !string.IsNullOrEmpty(mdlAdmin.Name) ? mdlAdmin.Name : admin.Name;
            admin.Email = !string.IsNullOrEmpty(mdlAdmin.Email) ? mdlAdmin.Email : admin.Email;
            admin.Password = !string.IsNullOrEmpty(mdlAdmin.Password) ? mdlAdmin.Password : admin.Password; 
            if (mdlAdmin.ImageData != null && mdlAdmin.ImageData.Length > 0)
            {
                using var stream = new MemoryStream();
                await mdlAdmin.ImageData.CopyToAsync(stream);
                admin.ImageData = stream.ToArray();
            }
            else
            {
                admin.ImageData = oldImageData; 
            }

            
            admin.UpdatedAt = DateTime.Now;
            await _appDbContext.SaveChangesAsync();

           
            return Ok(new
            {
                Message = "Admin data updated successfully!",
                UpdatedData = new
                {
                    admin.AdminId,
                    admin.Name,
                    admin.Email,
                    admin.UpdatedAt
                }
            });
        }

        #endregion


        // Method delete

        #region Delate Admin
        [HttpDelete("Delate_Admin/{adminId}")]
        public async Task<IActionResult> DeleteAdmin(string adminId)
        {
            var adminToDelete = await _appDbContext.Admins.FindAsync(adminId);
            if (adminToDelete == null)
            {
                return NotFound("This Admin is not found , maybe delete before!");
            }
             _appDbContext.Remove(adminToDelete);
            await _appDbContext.SaveChangesAsync();
            return Ok("admin is delete!");
        }
        #endregion
        // Owners Method




        // add Owners Acount 
        [HttpPost("AddOnewrsByAdmin")]
        //public async Task<IActionResult> AddOwner([FromForm] MdlOwner mdlOwner)
        //{
        //    using var stream = new MemoryStream();
        //    await mdlOwner.ImageData.CopyToAsync(stream);
        //    var owner = new BusinessOwner()
        //    {
        //        Name = mdlOwner.Name,
        //        Email = mdlOwner.Email,
        //        Password = mdlOwner.Password,
        //        PhoneNumper = mdlOwner.PhoneNumper,
        //        Rating = mdlOwner.Rating,
        //        TimeAdd = DateTime.Now,
        //        ImageData = stream.ToArray(),
        //        UniqueIdImage = Guid.NewGuid(),
        //        AdminId = mdlOwner.AdminId,

        //    };
        //    await _appDbContext.AddAsync(owner);
        //    await _appDbContext.SaveChangesAsync();
        //    return Ok(owner);


        //}

        // ----------------- Get all Owners -------------------
        #region Get All Owners
        [HttpGet("GetAllOwners")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> GetAllOwners()
        {
            var owners = await _appDbContext.BusinessOwners.ToListAsync();
            return Ok(owners);
        }
        #endregion

        // Create acount for Owner
        #region Creat Owner Acount

        [HttpPost("CreatOwnerAcount/{adminId}")]
        [Authorize(policy:"AdminOnly")]
        public async Task<IActionResult> AddNewOwner(string adminId,[FromForm] MdlOwner newOwner)
        {
            if (ModelState.IsValid)
            {
                var currrentadmin = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currrentadmin == null || currrentadmin != adminId)
                {
                    return Forbid("You do not have access to add admin!");
                }

                using var stream = new MemoryStream();
                await newOwner.ImageData.CopyToAsync(stream);
                var owner = new BusinessOwner()
                {
                    Name = newOwner.Name,
                    Email = newOwner.Email,
                    Password = newOwner.Password,
                    PhoneNumper = newOwner.PhoneNumper,
                    TimeAdd = DateTime.Now,
                    ImageData = stream.ToArray(),
                    UniqueIdImage = Guid.NewGuid(),
                };


                IdentityResult result = await _ownerManager.CreateAsync(owner, newOwner.Password);
                if (result.Succeeded)
                    return Ok("Sucsess");
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }
        #endregion
        // ----------------- Delete Owner -------------------
        #region Delate Owner
        [HttpDelete("Delate_Owner/{ownerId}")]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {
            var ownerToDelete = await _appDbContext.BusinessOwners.FindAsync(ownerId);
            if (ownerToDelete == null)
            {
                return NotFound("This Owner is not found , maybe delete before!");
            }
            _appDbContext.Remove(ownerToDelete);
            await _appDbContext.SaveChangesAsync();
            return Ok("Owner is delete!");
        }

        #endregion

        // --------------- Update Owner -------------
        #region Update Owner Info
        [HttpPut("Update_Owner_Info/{ownerId}")]
        public async Task<IActionResult> UpdateOwner(string ownerId, [FromForm] MdlOwner mdlOwner)
        {
            // Retrieve admin data from the database
            var owner = await _appDbContext.Admins.FindAsync(ownerId);
            if (owner == null)
            {
                return NotFound(new { Message = "This Owner was not found!" });
            }

            // Send existing data before updating
            var existingData = new
            {
                
                owner.Name,
                owner.Email,
                owner.Password,
                owner.PhoneNumper
               
            };

            //  If no new data is provided, return the existing data
            if (mdlOwner.Name == null && mdlOwner.Email == null && mdlOwner.PhoneNumper == null && mdlOwner.Password == null && mdlOwner.ImageData == null)
            {
                return Ok(new
                {
                    Message = "Current admin data retrieved successfully!",
                    ExistingData = existingData
                });
            }


            byte[] oldImageData = owner.ImageData;


            owner.Name = !string.IsNullOrEmpty(mdlOwner.Name) ? mdlOwner.Name : owner.Name;
            owner.Email = !string.IsNullOrEmpty(mdlOwner.Email) ? mdlOwner.Email : owner.Email;
            owner.Password = !string.IsNullOrEmpty(mdlOwner.Password) ? mdlOwner.Password : owner.Password;
            owner.PhoneNumper = !string.IsNullOrEmpty(mdlOwner.PhoneNumper) ? mdlOwner.PhoneNumper : owner.PhoneNumper;
            if (mdlOwner.ImageData != null && mdlOwner.ImageData.Length > 0)
            {
                using var stream = new MemoryStream();
                await mdlOwner.ImageData.CopyToAsync(stream);
                owner.ImageData = stream.ToArray();
            }
            else
            {
                owner.ImageData = oldImageData;
            }


           
            await _appDbContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Admin data updated successfully!",
                UpdatedData = new
                {
                    owner.Password,
                    owner.Name,
                    owner.Email,
                    owner.PhoneNumper
                }
            });
        }
        #endregion

        // --------- get Owers by name ------------
        // get all Owners from Admin controller
        #region Get Owner By Name
        [HttpGet("GetOwnerByName")]
        public async Task<IActionResult> GetOwnerByName([FromQuery] string? ownerName)
        {

            if (string.IsNullOrWhiteSpace(ownerName))
            {
                var Owners = await _appDbContext.BusinessOwners.ToListAsync();
                return Ok(Owners);
            }
            var ownerByName = await _appDbContext.BusinessOwners.Where(o => o.Name.Trim().ToLower().Contains(ownerName.Trim().ToLower())).ToListAsync();
            if (!ownerByName.Any())
            {
                return NotFound("The destination is not found!");
            }
            return Ok(ownerByName);

        }
        #endregion


        // --------------------------Feedback------------------------
        #region Get Feedbacks By UserId
        [HttpGet("GetFeedbacksByUserId")]
        public async Task<IActionResult> GetFeedbacks([FromQuery] string? contant)
        {

            if (string.IsNullOrWhiteSpace(contant))
            {
                var comment = await _appDbContext.Feedbacks.ToListAsync();
                return Ok(comment);
            }
            var commentByContent = await _appDbContext.Feedbacks.Where(o => o.Content.Trim().ToLower().Contains(contant)).ToListAsync();
            if (!commentByContent.Any())
            {
                return NotFound("The destination is not found!");
            }
            return Ok(commentByContent);

        }
        #endregion

        // Registration Methods 
        #region Admin Regesteration
        [HttpPost("AdminRegesteration")]
        [AllowAnonymous]
        
        public async Task<IActionResult> AddNewAdmin([FromForm] MdlAdmin newAdmin)
        {
            if (ModelState.IsValid)
            {
                using var stream = new MemoryStream();
                await newAdmin.ImageData.CopyToAsync(stream);
                var admin = new Admin()
                {
                    Name = newAdmin.Name,
                    Email = newAdmin.Email,
                    Password = newAdmin.Password,
                    PhoneNumper = newAdmin.PhoneNumper,
                    CreatedAt = DateTime.Now,
                    ImageData = stream.ToArray(),
                    
                };


                IdentityResult result = await _adminManager.CreateAsync(admin, newAdmin.Password);
                if (result.Succeeded)
                    return Ok("Sucsess");
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }
        #endregion


        #region Admin Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]MdlLogin login)
        {
            if (ModelState.IsValid)
            {
                Admin admin = await _adminManager.FindByEmailAsync(login.Email);
                if (admin != null)
                {
                    if (await _adminManager.CheckPasswordAsync(admin, login.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Email, admin.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        claims.Add(new Claim("UserType", "Admin"));
                        var roles = await _adminManager.GetRolesAsync(admin);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken
                        (
                           claims: claims,
                           issuer: _configuration["JWT:issuer"],
                           audience: _configuration["JWT:Audience"],
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
                    ModelState.AddModelError("", "admin Email is not valid!");
                }
            }
            return BadRequest(ModelState);
        }
        #endregion
        //-------------------------------------------------
    }
}
