using AbleEaseDomain.Enums;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // WEAK ENTITY: PROGRAM (depends on Organization)
    // ============================================
    public class Program
    {
        // Composite Key: OrganizationSSN + Id
        public Guid OrganizationSSN { get; set; }
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public ProgressStatus status { get; set; }

        public double price { get; set; }

        public List<Patient> patients { get; set; } = new List<Patient>();

        // Required parent (identifying relationship)
        public Organization organization { get; set; } = null!;

        // WEAK ENTITY: Assessment depends on Program (One-to-One)
        public Assessment? Assessment { get; set; }

        public List<Report> reports { get; set; } = new List<Report>();
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}