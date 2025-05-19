namespace YallaR7la.Data.Models
{
   
        namespace YallaR7la.Data.Models
        {
            public class Message
            {
                public string MessageId { get; set; } =Guid.NewGuid().ToString();
                public string Text { get; set; }
                public DateTime When { get; set; }

                public string ChatId { get; set; }
                public virtual Chat Chat { get; set; }

                public string SenderId { get; set; }   // Id of sender
                public string SenderRole { get; set; } // "User", "Owner", "Admin"

                // Navigation properties (make them nullable)
                public string? UserId { get; set; }
                public virtual User? User { get; set; }

                public string? BusinessOwnerId { get; set; }
                public virtual BusinessOwner? Owner { get; set; }

                public string? AdminId { get; set; }
                public virtual Admin? Admin { get; set; }
            }
        }



}
