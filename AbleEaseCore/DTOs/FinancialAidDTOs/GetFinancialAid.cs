using AbleEaseDomain.Enums;

namespace AbleEaseCore.DTOs.FinancialAidDTOs
{
    public class GetFinancialAid
    {
        public Guid Id { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public double percentage { get; set; }
        public DateTime ApplicationDate { get; set; }
        public Guid? PatientSSN { get; set; }
        public Guid? OrganizationSSN { get; set; }
    }
}
