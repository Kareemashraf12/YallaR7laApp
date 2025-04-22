using System.ComponentModel.DataAnnotations;

namespace YallaR7la.Data.Models
{
    public class Admin
    {
        [Key]
        public string AdminId { get; set; }

        [Required(ErrorMessage = "Admin name is required.")]
        [StringLength(100, ErrorMessage = "Admin name cannot be more than 100 characters.")]
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public byte[] ImageData { get; set; }
        public Guid UniqeImageId { get; set; } = Guid.NewGuid();

        // relations
        public ICollection<BusinessOwner> Business { get; set; }
        public ICollection<AnalyticsReport> Analytics { get; set; }
    }
}
