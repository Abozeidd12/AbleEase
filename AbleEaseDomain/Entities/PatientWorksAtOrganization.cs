namespace AbleEaseDomain.Entities
{
    // ============================================
    // JOIN TABLE: PATIENT-ORGANIZATION (Work Relationship)
    // ============================================
    public class PatientWorksAtOrganization
    {
        public Guid PatientSSN { get; set; }
        public Patient? patient { get; set; }

        public Guid OrganizationSSN { get; set; }
        public Organization? organization { get; set; }

        public string? jobTitle { get; set; }
        public double salary { get; set; }
        public DateTime startDate { get; set; }
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}