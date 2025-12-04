namespace AbleEaseDomain.Entities
{
    // ============================================
    // JOIN TABLE: ASSESSMENT-PATIENT
    // ============================================
    public class AssessmentPatient
    {
        public Guid PatientSSN { get; set; }
        public Patient? patient { get; set; }

        // Reference to Assessment (WEAK ENTITY) - Need composite FK
        public Guid AssessmentProgramOrganizationSSN { get; set; }
        public int AssessmentProgramId { get; set; }
        public int AssessmentId { get; set; }
        public Assessment? Assessment { get; set; }

        public double Grade { get; set; }
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}