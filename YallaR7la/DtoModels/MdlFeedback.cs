using System.ComponentModel.DataAnnotations;

namespace YallaR7la.DtoModels
{
    public class MdlFeedback
    {
        [Required(ErrorMessage = "Feedback content is required.")]
        [StringLength(1000, ErrorMessage = "Feedback content cannot exceed 1000 characters.")]
        public string Content { get; set; }


        public int Rating { get; set; }
    }
}
