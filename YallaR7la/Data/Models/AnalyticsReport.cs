using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YallaR7la.Data.Models
{
    public class AnalyticsReport
    {
    
        [Key]
        public string ReportId { get; set; }

        
        [Required]
        public string ReportData { get; set; } // JSON format for analytics

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // relations 

        [Required]
        [ForeignKey(nameof(BusinessOwner))]
        public string OwnerId { get; set; }
        public virtual BusinessOwner BusinessOwner { get; set; }

        [Required]
        [ForeignKey(nameof(Admin))]
        public string GeneratedByAdminId { get; set; }
        public virtual Admin Admin { get; set; }

    }

}

