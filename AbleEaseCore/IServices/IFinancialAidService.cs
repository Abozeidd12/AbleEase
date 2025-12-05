using AbleEaseCore.DTOs.FinancialAidDTOs;
using AbleEaseDomain.Enums;

namespace AbleEaseCore.IServices
{
    public interface IFinancialAidService
    {
        Task<GetFinancialAid> AddFinancialAidAsync(AddFinancialAid addFinancialAid);
        Task<GetFinancialAid?> GetFinancialAidByIDAsync(Guid id);
        Task<IEnumerable<GetFinancialAid>> GetAllFinancialAidsAsync();

        Task<GetFinancialAid> UpdateFinancialAidAsync(Guid id, UpdateFinancialAid updateFinancialAid);
        Task<bool> DeleteFinancialAidAsync(Guid id);
        Task<(IEnumerable<GetFinancialAid> FinancialAids, int totalCount)> GetPagedFinancialAidsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<GetFinancialAid>> GetFinancialAidsByPatientAsync(Guid patientSsn);
        Task<IEnumerable<GetFinancialAid>> GetFinancialAidsByOrganizationAsync(Guid organizationSsn);
        Task<IEnumerable<GetFinancialAid>> GetFinancialAidsByStatusAsync(ApprovalStatus status);
        Task<IEnumerable<GetFinancialAid>> GetPendingFinancialAidsAsync();
    }
}
