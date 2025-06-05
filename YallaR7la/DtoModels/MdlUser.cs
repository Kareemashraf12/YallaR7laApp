using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlUser
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
        public string? Prefrance { get; set; }
        public string? City { get; set; }
        
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = "Invalid phone number format.")]
        
        public string? PhoneNumper { get; set; }
        public DateTime BirthDate { get; set; }

        public IFormFile? ImageData { get; set; }
      

    }
}
