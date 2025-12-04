namespace AbleEaseDomain.Entities
{
    // ============================================
    // WEAK ENTITY: ASSESSMENT (depends on Program)
    // ============================================
    public class Assessment
    {
        // Composite Key: ProgramOrganizationSSN + ProgramId + Id
        public Guid ProgramOrganizationSSN { get; set; }
        public int ProgramId { get; set; }
        public int Id { get; set; }

        // Required parent (identifying relationship)
        public Program program { get; set; } = null!;

        public List<AssessmentPatient> assessmentPatients { get; set; } = new List<AssessmentPatient>();
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}