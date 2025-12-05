using AbleEaseDomain.Enums;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // RELATIVE
    // ============================================
    public class Relative : User
    {
        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public Patient? Patient { get; set; }

        // Can view patient's medical info
        public List<MedicalInfo> medicalInfo { get; set; } = new List<MedicalInfo>();
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}