using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;
using YallaR7la.DtoModels;
using YallaR7la.Services;

namespace YallaR7la.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailingControler : ControllerBase
    {

        private readonly IEmailServices _emailServices;
        private readonly IMemoryCache _memoryCache;

        public MailingControler(IEmailServices emailServices , IMemoryCache memoryCache)
        {
            _emailServices = emailServices;
            _memoryCache = memoryCache;
            
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] MdlEmailRequest emailRequest)
        {
            string verificationCode = GenerateVerificationCode(6);

            string subject = "Your Email Verification Code";
            string body = $"Your verification code is: {verificationCode}";

            await _emailServices.SendEmailAsync(emailRequest.ToEmail, subject, body);

            _memoryCache.Set("CurrentEmail", emailRequest.ToEmail, TimeSpan.FromMinutes(5)); // Save email
            _memoryCache.Set(emailRequest.ToEmail, verificationCode, TimeSpan.FromMinutes(5)); // Save code

            return Ok(new { Message = "Verification email sent successfully." });
        }

        [HttpPost("VerifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] MdlVerifyRequest verifyRequest)
        {
            if (_memoryCache.TryGetValue("CurrentEmail", out string currentEmail))
            {
                if (_memoryCache.TryGetValue(currentEmail, out string storedCode))
                {
                    if (storedCode == verifyRequest.Code)
                    {
                        _memoryCache.Remove(currentEmail);    // Clear verification code
                        _memoryCache.Remove("CurrentEmail");   // Clear stored email
                        return Ok(new { Message = "Verification successful!" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "Incorrect verification code." });
                    }
                }
            }

            return BadRequest(new { Message = "Verification code expired or email not found." });
        }




        //  method to generate random 6-digit code
        private string GenerateVerificationCode(int length)
        {
            const string chars = "0123456789";
            StringBuilder code = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[length];
                rng.GetBytes(bytes);
                foreach (byte b in bytes)
                {
                    code.Append(chars[b % chars.Length]);
                }
            }
            return code.ToString();
        }

    }
}
