using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlOwner
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Number is Required")]
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = "Invalid phone number format.")]

        public string PhoneNumper { get; set; }

        [Required]
        public DateTime TimeAdd { get; set; }
        public IFormFile ImageData { get; set; }
        public Guid UniqueIdImage { get; set; }
        public string AdminId { get; set; }

     

    }
}
