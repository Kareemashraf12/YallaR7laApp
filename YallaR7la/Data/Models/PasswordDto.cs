using System.ComponentModel.DataAnnotations;

namespace YallaR7la.Data.Models
{
    public class PasswordDto
    {
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}
