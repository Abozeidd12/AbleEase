using System.ComponentModel.DataAnnotations;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // DISABILITY
    // ============================================
    public class Disability
    {
        [Key]
        public Guid SSN { get; set; }

        public string? name { get; set; }
        public string? type { get; set; }
        public string? description { get; set; }

        public List<PatientDisability> patientDisabilities { get; set; } = new List<PatientDisability>();
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}