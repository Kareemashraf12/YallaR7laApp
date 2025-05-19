using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlResetPassword
    {
       
       

        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters long.")]
        public string NewPassword { get; set; }
    }

}
