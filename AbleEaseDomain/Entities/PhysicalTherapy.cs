using System.ComponentModel.DataAnnotations;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // PHYSICAL THERAPY
    // ============================================
    public class PhysicalTherapy
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public int duration { get; set; }
        
        public string? Doctorname { get; set; }
        public string? therapyDetails { get; set; }

        public Guid? PatientSSN { get; set; }
        public Patient? patient { get; set; }

        public Guid? CenterID { get; set; }

        public PhysiotherapyCenter? center { get; set; }
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}