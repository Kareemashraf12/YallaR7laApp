using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YallaR7la.Data;
using YallaR7la.Data.Models;
using YallaR7la.Data.Models.YallaR7la.Data.Models;
using YallaR7la.DtoModels;

namespace YallaR7la.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessagesController(AppDbContext context)
        {
            _context = context;
        }



        #region CreateChat
        [HttpPost("CreateChat")]
        [Authorize]
        public async Task<IActionResult> CreateChat([FromQuery] string? ownerId = null, [FromQuery] string? adminId = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                return Unauthorized("Invalid user or role.");

            // Validate roles and chat targets based on rules:
            var chat = new Chat();

            if (userRole == "User")
            {
                // User can chat with either Owner or Admin, must specify one
                if (!string.IsNullOrEmpty(ownerId))
                {
                    var ownerExists = await _context.BusinessOwners.AnyAsync(o => o.BusinessOwnerId == ownerId);
                    if (!ownerExists) return NotFound("Owner not found.");

                    chat.UserId = userId;
                    chat.OwnerId = ownerId;

                    // Admin is required for chat, assign default admin
                    chat.AdminId = await _context.Admins.Select(a => a.AdminId).FirstOrDefaultAsync();
                }
                else if (!string.IsNullOrEmpty(adminId))
                {
                    var adminExists = await _context.Admins.AnyAsync(a => a.AdminId == adminId);
                    if (!adminExists) return NotFound("Admin not found.");

                    chat.UserId = userId;
                    chat.AdminId = adminId;
                }
                else
                {
                    return BadRequest("User must specify either OwnerId or AdminId to chat with.");
                }
            }
            else if (userRole == "Owner")
            {
                // Owner can only start chat with Admin, must specify adminId
                if (string.IsNullOrEmpty(adminId))
                    return BadRequest("Owner must specify AdminId to start chat.");

                var adminExists = await _context.Admins.AnyAsync(a => a.AdminId == adminId);
                if (!adminExists) return NotFound("Admin not found.");

                chat.OwnerId = userId;
                chat.AdminId = adminId;
            }
            else if (userRole == "Admin")
            {
                // Admin can only start chat with Owner, must specify ownerId
                if (string.IsNullOrEmpty(ownerId))
                    return BadRequest("Admin must specify OwnerId to start chat.");

                var ownerExists = await _context.BusinessOwners.AnyAsync(o => o.BusinessOwnerId == ownerId);
                if (!ownerExists) return NotFound("Owner not found.");

                chat.AdminId = userId;
                chat.OwnerId = ownerId;
            }
            else
            {
                return Forbid("Invalid role for creating chat.");
            }

            _context.Chat.Add(chat);
            await _context.SaveChangesAsync();

            return Ok(new { ChatId = chat.ChatId, Message = "Chat created successfully!" });
        }


        #endregion



        #region SendMessage

        [HttpPost("SendMessage")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                return Unauthorized("Invalid user or role.");

            var chat = await _context.Chat.Include(c => c.Messages)
                                          .FirstOrDefaultAsync(c => c.ChatId.ToString() == request.ChatId);

            if (chat == null)
                return NotFound("Chat not found.");

            var message = new Message
            {
                ChatId = request.ChatId,
                Text = request.Text,
                When = DateTime.UtcNow,
                SenderId = userId,
                SenderRole = userRole
            };

            // Set sender navigation property based on role
            switch (userRole)
            {
                case "User":
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null) return BadRequest("Invalid user.");
                    message.UserId = userId;
                    message.BusinessOwnerId = null;
                    message.AdminId = null;
                    break;

                case "Owner":
                    var owner = await _context.BusinessOwners.FindAsync(userId);
                    if (owner == null) return BadRequest("Invalid business owner.");
                    message.BusinessOwnerId = userId;
                    message.UserId = null;
                    message.AdminId = null;
                    break;

                case "Admin":
                    var admin = await _context.Admins.FindAsync(userId);
                    if (admin == null) return BadRequest("Invalid admin.");
                    message.AdminId = userId;
                    message.BusinessOwnerId = null;
                    message.UserId = null;
                    break;

                default:
                    return Forbid("Unknown sender role.");
            }

            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Message sent successfully", message.MessageId });
        }



        #endregion



        #region GetMyChats

        [HttpGet("GetChats")]
        [Authorize]
        public async Task<IActionResult> GetMyChats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                return Unauthorized();

            IQueryable<Chat> query = _context.Chat.Include(c => c.Messages);

            if (userRole == "User")
                query = query.Where(c => c.UserId == userId);
            else if (userRole == "Owner")
                query = query.Where(c => c.OwnerId == userId);
            else if (userRole == "Admin")
                query = query.Where(c => c.AdminId == userId);
            else
                return Forbid();

            var result = await query.Select(c => new
            {
                c.ChatId,
                c.CreatedAt,
                Messages = c.Messages.OrderBy(m => m.When).Select(m => new
                {
                    m.MessageId,
                    m.Text,
                    m.When,
                    m.SenderId,
                    m.SenderRole
                })
            }).ToListAsync();

            return Ok(result);
        }


        #endregion

        #region DeleteMessagesByChat

        [HttpDelete("DeleteChatWithMessages/{chatId}")]
        [Authorize]
        public async Task<IActionResult> DeleteChatWithMessages(string chatId)
        {
            var chat = await _context.Chat.Include(c => c.Messages)
                                          .FirstOrDefaultAsync(c => c.ChatId == chatId);
            if (chat == null)
                return NotFound("Chat not found.");

            _context.Message.RemoveRange(chat.Messages);
            _context.Chat.Remove(chat);

            await _context.SaveChangesAsync();

            return Ok("Chat and its messages deleted.");
        }


        #endregion


        #region DeleteMessage

        [HttpDelete("DeleteMessage/{messageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            var message = await _context.Message.FindAsync(messageId);
            if (message == null)
            {
                return NotFound("Message not found.");
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return Ok("Message deleted successfully.");
        }


        #endregion


    }


}