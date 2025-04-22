using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlFeedback
    {
        [Required(ErrorMessage = "Feedback content is required.")]
        [StringLength(1000, ErrorMessage = "Feedback content cannot exceed 1000 characters.")]
        public string Content { get; set; }
        
        [StringLength(20, ErrorMessage = "Sentiment score cannot be exeed 20 characters.")]
        public string SentimentScore { get; set; }
        public string DestinationId { get; set; }
        public string UserId { get; set; }
    }
}
