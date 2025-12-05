using AbleEaseDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // PAYMENT
    // ============================================
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime date { get; set; }
        public double amount { get; set; }
        public ApprovalStatus approvalStatus { get; set; }

        public Guid? PatientSSN { get; set; }
        public Patient? patient { get; set; }

        public Guid? FinancialId { get; set; }
        public FinancialAid? financialAid { get; set; }
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}