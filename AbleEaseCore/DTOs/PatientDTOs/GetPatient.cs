using AbleEaseDomain.Entities;
using AbleEaseDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AbleEaseCore.DTOs.PatientDTOs
{
    public class GetPatient
    {
        [Required]
        public Guid SSN { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ContactInfo { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid? RelativeSSN { get; set; }
        public string? RelativeName { get; set; }

        public Guid? CaregiverSSN { get; set; }
        public string? CaregiverName { get; set; }

        public Guid? ProgramOrganizationSSN { get; set; }
        public int? ProgramId { get; set; }
        public string? ProgramName { get; set; }

        // Collections (optional - include only if needed)
        //public List<GetMedicalInfo>? MedicalInfo { get; set; }
        //public List<GetDisability>? Disabilities { get; set; } 
        //public List<GetReport>? Reports { get; set; }
        //public List<GetFinancialAid>? FinancialAids { get; set; }
        //public List<GetPhysicalTherapy>? PhysicalTherapies { get; set; }
        // Note: Payment, AssessmentPatient, and worksAt are omitted for brevity,
        // but would follow the same pattern if required by the API consumer.
    }
}
