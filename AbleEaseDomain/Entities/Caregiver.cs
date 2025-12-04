using AbleEaseDomain.Enums;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // CAREGIVER
    // ============================================
    public class Caregiver : User
    {
        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public List<Report> reports { get; set; } = new List<Report>();

        public Guid? OrganizationSSN { get; set; }
        public Organization? organization { get; set; }

        public List<Patient> patients { get; set; } = new List<Patient>();
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}