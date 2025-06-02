using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YallaR7la.Data.Models
{
    public class Booked
    {
        public string BookedId { get; set; } = Guid.NewGuid().ToString();

        public DateTime BookedDate { get; set; } = DateTime.UtcNow;


        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [Required]
        [ForeignKey("Destination")]
        public string DestinationId { get; set; }

        public Destination Destination { get; set; }


    }
}
