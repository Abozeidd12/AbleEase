using System.ComponentModel.DataAnnotations;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // PHYSIOTHERAPY CENTER
    // ============================================
    public class PhysiotherapyCenter
    {
        [Key]
        public Guid SSN { get; set; }

        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? ContactInfo { get; set; }

        public List<PhysicalTherapy> physicalTherapies = new List<PhysicalTherapy> ();

    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}