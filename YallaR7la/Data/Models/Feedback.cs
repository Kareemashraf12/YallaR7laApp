using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using YallaR7la.Data.Models;

public class Feedback
{
    [Key]
    public string FeedbackId { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Feedback content is required.")]
    [StringLength(1000, ErrorMessage = "Feedback content cannot exceed 1000 characters.")]
    public string Content { get; set; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [Required]
    public DateTime dataSubmited { get; set; } 

    [Required]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; }
    public virtual User User { get; set; }

    [Required]
    [ForeignKey(nameof(Destination))]
    public string DestinationId { get; set; }
    public Destination Destination { get; set; }
}
