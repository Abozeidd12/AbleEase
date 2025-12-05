namespace AbleEaseDomain.Entities
{
    // ============================================
    // JOIN TABLE: PATIENT-DISABILITY
    // ============================================
    public class PatientDisability
    {
        public Guid PatientSSN { get; set; }
        public Patient? patient { get; set; }

        public Guid DisabilityID { get; set; }
        public Disability? disability { get; set; }

        public string? level { get; set; }
        public string? notes { get; set; }
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}