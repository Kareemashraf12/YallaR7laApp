using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using YallaR7la.Data; // Your DbContext namespace
using YallaR7la.DtoModels;

namespace YallaR7la.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _dbContext; // Database context

        public VerificationController(IMemoryCache memoryCache, IEmailSender emailSender, AppDbContext dbContext)
        {
            _memoryCache = memoryCache;
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        [HttpPost("SendVerificationCode")]
        [AllowAnonymous]
        public async Task<IActionResult> SendVerificationCode([FromBody] MdlSendVerificationCode dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check which table the email belongs to
            string role = GetUserRole(dto.Email);

            if (role == null)
                return BadRequest("Email not found in the system.");

            // Generate a 6-digit random verification code
            string code = new Random().Next(100000, 999999).ToString();

            // Store the code in cache for 10 minutes
            _memoryCache.Set(dto.Email, code, TimeSpan.FromMinutes(10));

            // Send the verification code via email
            await _emailSender.SendEmailAsync(dto.Email, "Your Verification Code", $"Your verification code is: {code}");

            return Ok(new { Message = "Verification code sent successfully.", Role = role });
        }

        [HttpPost("VerifyCode")]
        [AllowAnonymous]
        public IActionResult VerifyCode([FromBody] MdlVerifyCode dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Try to retrieve the verification code from the cache
            if (!_memoryCache.TryGetValue(dto.Email, out string cachedCode))
            {
                return BadRequest("Verification code expired or not found. Please request a new one.");
            }

            // Compare the code provided by the user with the cached code
            if (dto.VerificationCode != cachedCode)
            {
                return BadRequest("Invalid verification code.");
            }

            // Remove the code from the cache after successful verification
            _memoryCache.Remove(dto.Email);

            return Ok("Verification successful.");
        }

        // Function to check which table the email belongs to
        private string GetUserRole(string email)
        {
            if (_dbContext.Admins.Any(a => a.Email == email))
                return "Admin";

            if (_dbContext.Users.Any(u => u.Email == email))
                return "User";

            if (_dbContext.BusinessOwners.Any(o => o.Email == email))
                return "Owner";

            return null; // Email not found in any table
        }
    }
}
