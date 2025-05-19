using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YallaR7la.Data.Models
{
    public class Destination
    {
        [Key]
        public string DestinationId { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage ="Destination name is required")]
        [StringLength(100,ErrorMessage ="destination name can not be more than 100 characters.")]
        public string Name { get; set; }
        [StringLength(500,ErrorMessage ="Description cannot be more 500 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage ="Location is requiesd.")]
        [StringLength(200 ,ErrorMessage ="Location cannot be more than 200 characters")]
        public string Location { get; set; }
        [Required(ErrorMessage ="Category is required.")]
        [StringLength(50,ErrorMessage ="Category cannot be more than 50 characters.")]
        public string Category { get; set; }
        [Required(ErrorMessage = "The number avelable is required.")]
        public int AvilableNumber { get; set; }
        [Range(1, 5, ErrorMessage = "Average rating must be between 1 and 5.")]
        public int? AverageRating { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="Feedback count must be positive number.")]
        public int? FeedbackCount { get; set; }

        [Range(1,5 ,ErrorMessage ="Rating must be between 1 and 5.")]
        public int? Rating   { get; set; }
        [Required]
        public DateTime TimeAdd { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage ="Start date is Required.")]
        //[DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is Required.")]
        //[DataType(DataType.Date)]
        public DateTime EndtDate { get; set; }
        public bool IsAvelable { get; set; }=false;
        
        [Column(TypeName ="decimal(18,2)")]
        public decimal Cost { get; set; }

        public decimal Discount { get; set; } = 0;

        // relations

        [Required]
        [ForeignKey(nameof(BusinessOwner))]
        public string BusinessOwnerId { get; set; }
        public BusinessOwner BusinessOwner { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }
        // Navigation property for many-to-many relation
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<DestinationImages> destinationImages { get; set; }

    }
}
