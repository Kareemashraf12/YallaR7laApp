using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlEmailRequest
    {
        [Required]
        public string ToEmail { get; set; }
        
        
        
    }
}
