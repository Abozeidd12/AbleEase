using System.ComponentModel.DataAnnotations;

namespace AbleEaseCore.DTOs.FinancialAidDTOs
{
    public class AddFinancialAid
    {
        [Required(ErrorMessage = "PatientSSN is required.")]
        public Guid PatientSSN { get; set; }
        //[Required(ErrorMessage = "OrganizationSSN is required.")]
        public Guid? OrganizationSSN { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
        public double percentage { get; set; }
    }
}
