using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlSendVerificationCode
    {
        [Required]
        public string Email { get; set; }
    }

    public class MdlVerifyCode
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}

