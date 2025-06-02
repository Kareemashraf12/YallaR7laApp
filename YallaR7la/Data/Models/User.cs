using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using YallaR7la.Data.Models.YallaR7la.Data.Models;

namespace YallaR7la.Data.Models
{
    public class User:IdentityUser
    {
       
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
       
        public string Prefrance { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Number is Required")]
        [RegularExpression(@"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}", ErrorMessage = "Invalid phone number format.")]

        public string PhoneNumper { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate.Date < today.AddYears(-age))
                    age--;
                return age;
            }
        }

        //public string UserRole { get; set; } // User - Owner - Admin
        public byte[] ImageData { get; set; }
       

        //relations 
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }


    }
}


