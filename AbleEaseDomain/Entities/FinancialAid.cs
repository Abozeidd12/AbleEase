using AbleEaseDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AbleEaseDomain.Entities
{
    public class FinancialAid
    {
        [Key]
        public Guid Id { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public double percentage { get; set; }
        public DateTime ApplicationDate { get; set; }
        public Guid? PatientSSN { get; set; }
        public Patient? patient { get; set; }
        public Guid? OrganizationSSN { get; set; }
        public Organization? organization { get; set; }
        public Payment? payment { get; set; }
    }
}