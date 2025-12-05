namespace AbleEaseDomain.Entities
{
    // ============================================
    // WEAK ENTITY: MEDICAL INFO (depends on Patient)
    // ============================================
    public class MedicalInfo
    {
        // Composite Key: PatientSSN + Id
        public Guid PatientSSN { get; set; }
        public int Id { get; set; }

        public string? DoctorName { get; set; }
        public string? Diagnosis { get; set; }
        public string? TherapyDeatils { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        // Required parent (identifying relationship)
        public Patient Patient { get; set; } = null!;

        // Non-identifying relationship
        public Guid? RelativeSSN { get; set; }
        public Relative? Relative { get; set; }
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}