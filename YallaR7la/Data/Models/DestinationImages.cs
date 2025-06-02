using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YallaR7la.Data.Models
{
    public class DestinationImages
    {
        
        
           [Key]
            public string ImageId { get; set; }

            [Required]
            public byte[] ImageData { get; set; } 

            

            [Required]
            [ForeignKey(nameof(Destination))]
            public string DestinationId { get; set; }

            
            public Destination Destination { get; set; }

            
        

    }
}
