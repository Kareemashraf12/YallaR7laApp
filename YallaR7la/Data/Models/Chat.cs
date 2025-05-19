using YallaR7la.Data.Models.YallaR7la.Data.Models;

namespace YallaR7la.Data.Models
{
    public class Chat
    {
        public string ChatId { get; set; } = Guid.NewGuid().ToString();

        // User and Owner are different tables
        public string? UserId { get; set; } // Nullable - if a Tourist started the chat
        public string? OwnerId { get; set; } // Nullable - if a Business Owner started the chat
        public string? AdminId { get; set; } // Always an Admin

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Message> Messages { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual BusinessOwner Owner { get; set; }
        public virtual Admin Admin { get; set; }
    }

}
