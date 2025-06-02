using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlDistanationImages
    {
        [Required]
        public List<IFormFile> ImageData { get; set; }
        public string DestinationId { get; set; }
    }
}
