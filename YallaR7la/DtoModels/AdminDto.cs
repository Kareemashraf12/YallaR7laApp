using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class AdminDto
    {

        [Required(ErrorMessage = "Admin name is required.")]
        [StringLength(100, ErrorMessage = "Admin name cannot be more than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "Number is Required")]
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumper { get; set; }
    }
}
