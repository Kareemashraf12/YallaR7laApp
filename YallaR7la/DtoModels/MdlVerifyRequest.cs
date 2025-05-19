using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlVerifyRequest
    {
 
        [Required]
        public string Code { get; set; }

    }
}
