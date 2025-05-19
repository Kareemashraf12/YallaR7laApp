using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlDestination
    {
        [Required(ErrorMessage = "Destination name is required")]
        [StringLength(100, ErrorMessage = "destination name can not be more than 100 characters.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot be more 500 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Location is requiesd.")]
        [StringLength(200, ErrorMessage = "Location cannot be more than 200 characters")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category cannot be more than 50 characters.")]
        public string Category { get; set; }
        [Required(ErrorMessage = "The number avelable is required.")]
        public int AvilableNumber { get; set; }
        

        
        
        [Required(ErrorMessage = "Start date is Required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is Required.")]
        [DataType(DataType.Date)]
        public DateTime EndtDate { get; set; }
       

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
        public decimal Discount { get; set; } = 0;
        
    }
}
