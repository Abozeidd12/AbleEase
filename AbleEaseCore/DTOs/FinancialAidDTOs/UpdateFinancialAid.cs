using AbleEaseDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AbleEaseCore.DTOs.FinancialAidDTOs
{
    public class UpdateFinancialAid
    {
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
        public double? percentage { get; set; }

        [EnumDataType(typeof(ApprovalStatus), ErrorMessage = "Invalid approval status")]
        public ApprovalStatus? ApprovalStatus { get; set; }
    }
}
