using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YallaR7la.Data.Models
{
    public class Feedback
    {
        [Key] 
        public string FeedbackId { get; set; }

        [Required(ErrorMessage ="Feedback content is required.")]
        [StringLength(1000,ErrorMessage ="Feedback content cannot exceed 1000 characters.")]
        public string Content { get; set; }
        [Required]
        public DateTime dataSubmited { get; set; } =DateTime.UtcNow;

        public bool IsReviewed { get; set; } = false;
        [StringLength(20,ErrorMessage = "Sentiment score cannot be exeed 20 characters.")]
        public string SentimentScore { get; set; }

        // relations 

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey(nameof(Destination))]
        public string DestinationId { get; set; }
        public Destination Destination { get; set; }
    }
}
