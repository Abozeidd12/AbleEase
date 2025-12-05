using System.ComponentModel.DataAnnotations;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // REPORT
    // ============================================
    public class Report
    {
        [Key]
        public Guid Id { get; set; }

        public string? Subject { get; set; }
        public string? Content { get; set; }
        public DateTime date { get; set; }

        public Guid? CaregiverSSN { get; set; }
        public Caregiver? caregiver { get; set; }

        public Guid? PatientSSN { get; set; }
        public Patient? patient { get; set; }

        // Reference to Program (WEAK ENTITY) - Need composite FK
        public Guid? ProgramOrganizationSSN { get; set; }
        public int? ProgramId { get; set; }
        public Program? program { get; set; }
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}