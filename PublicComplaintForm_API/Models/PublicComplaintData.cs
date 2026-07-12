using System.ComponentModel.DataAnnotations;

namespace PublicComplaintForm_API.Models
{
    public class PublicComplaintData
    {
        [Required]
        [MaxLength(7000)]
        public string ContactDescription { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9]+$")]
        public string CourtCaseNumber { get; set; } = string.Empty;

        public string Courthouse { get; set; } = string.Empty;
    }
}
