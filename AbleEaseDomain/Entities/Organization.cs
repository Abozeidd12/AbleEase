namespace AbleEaseDomain.Entities
{
    // ============================================
    // ORGANIZATION
    // ============================================
    public class Organization : User
    {
        

        // WEAK ENTITY: Programs depend on Organization
        public List<Program> Programs { get; set; } = new List<Program>();

        public List<FinancialAid> financialAids { get; set; } = new List<FinancialAid>();
        public List<Caregiver> caregivers { get; set; } = new List<Caregiver>();
        public List<PatientWorksAtOrganization> patientWorksAts { get; set; } = new List<PatientWorksAtOrganization>();
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}