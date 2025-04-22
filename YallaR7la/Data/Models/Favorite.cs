using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YallaR7la.Data.Models
{
    public class Favorite
    {
 
        [Key] 
        public string FavoriteId { get; set; }

        [Required] 
        public DateTime FavoritedAt { get; set; } = DateTime.UtcNow;

        // relations

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("Destination")]
        public string DestinationId { get; set; }
        public virtual Destination Destination { get; set; }
    }

}

