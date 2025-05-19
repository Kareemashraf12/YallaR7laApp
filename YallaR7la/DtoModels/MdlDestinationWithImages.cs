using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlDestinationWithImages
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int AvilableNumber { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndtDate { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public decimal Cost { get; set; }

        
        public IFormFile[]? ImageData { get; set; }
    }

}
