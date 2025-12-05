using AbleEaseCore.DTOs.FinancialAidDTOs;
using AbleEaseCore.IServices;
using AbleEaseDomain.Entities;
using AbleEaseDomain.Enums;
using AbleEaseDomain.IRepositeries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbleEaseCore.Services
{
    public class FinancialAidService : IFinancialAidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FinancialAidService> _logger;

        public FinancialAidService(IUnitOfWork unitOfWork, ILogger<FinancialAidService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GetFinancialAid> AddFinancialAidAsync(AddFinancialAid addFinancialAid)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Validate Patient exists
                var patientExists = await _unitOfWork.Repository<Patient>()
                    .ExistsAsync(p => p.SSN == addFinancialAid.PatientSSN);

                if (!patientExists)
                {
                    throw new ArgumentException($"Patient with SSN {addFinancialAid.PatientSSN} not found");
                }

                // Validate Organization exists
                var organizationExists = await _unitOfWork.Repository<Organization>()
                    .ExistsAsync(o => o.SSN == addFinancialAid.OrganizationSSN);

                //if (!organizationExists)
                //{
                //    throw new ArgumentException($"Organization with SSN {addFinancialAid.OrganizationSSN} not found");
                //}

                var id = Guid.NewGuid();

                var financialAid = new FinancialAid
                {
                    Id = id,
                    ApprovalStatus = ApprovalStatus.Pending,
                    percentage = addFinancialAid.percentage,
                    ApplicationDate = DateTime.UtcNow,
                    PatientSSN = addFinancialAid.PatientSSN,
                    //OrganizationSSN = addFinancialAid.OrganizationSSN
                };

                await _unitOfWork.Repository<FinancialAid>().AddAsync(financialAid);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("FinancialAid created successfully with ID: {Id}", id);

                return MapToGetFinancialAid(financialAid);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating FinancialAid");
                throw;
            }
        }

        public async Task<GetFinancialAid?> GetFinancialAidByIDAsync(Guid id)
        {
            try
            {
                // Simple query - no includes needed
                var financialAidEntity = await _unitOfWork.Repository<FinancialAid>()
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (financialAidEntity == null)
                    return null;

                return MapToGetFinancialAid(financialAidEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving FinancialAid with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<GetFinancialAid>> GetAllFinancialAidsAsync()
        {
            try
            {
                
                var financialAidEntities = await _unitOfWork.Repository<FinancialAid>()
                    .GetAllAsync();

                return financialAidEntities
                    .Select(MapToGetFinancialAid)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all FinancialAids");
                throw;
            }
        }

        public async Task<(IEnumerable<GetFinancialAid> FinancialAids, int totalCount)> GetPagedFinancialAidsAsync(
            int pageNumber, int pageSize)
        {
            try
            {
         
                var (financialAids, totalCount) = await _unitOfWork.Repository<FinancialAid>()
                    .GetPagedAsync(
                        pageNumber,
                        pageSize,
                        orderBy: q => q.OrderByDescending(f => f.ApplicationDate)
                    );

                var result = financialAids
                    .Select(MapToGetFinancialAid)
                    .ToList();

                return (result, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged FinancialAids");
                throw;
            }
        }

        public async Task<GetFinancialAid> UpdateFinancialAidAsync(Guid id, UpdateFinancialAid updateFinancialAid)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var financialAid = await _unitOfWork.Repository<FinancialAid>()
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (financialAid == null)
                    throw new KeyNotFoundException($"FinancialAid with ID {id} not found");

                if (updateFinancialAid.percentage.HasValue && financialAid.ApprovalStatus != ApprovalStatus.Pending)
                {
                    throw new InvalidOperationException(
                        $"Cannot update percentage for financial aid with status: {financialAid.ApprovalStatus}. Only pending applications can be modified.");
                }

                // Apply updates
                if (updateFinancialAid.percentage.HasValue)
                {
                    if (updateFinancialAid.percentage.Value < 0 || updateFinancialAid.percentage.Value > 100)
                    {
                        throw new ArgumentException("Percentage must be between 0 and 100");
                    }
                    financialAid.percentage = updateFinancialAid.percentage.Value;
                }

                if (updateFinancialAid.ApprovalStatus.HasValue)
                {
                    financialAid.ApprovalStatus = updateFinancialAid.ApprovalStatus.Value;
                }

                await _unitOfWork.Repository<FinancialAid>().UpdateAsync(financialAid);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("FinancialAid updated successfully with ID: {Id}", id);

                return MapToGetFinancialAid(financialAid);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating FinancialAid with ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteFinancialAidAsync(Guid id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var financialAid = await _unitOfWork.Repository<FinancialAid>()
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (financialAid == null)
                    return false;

                await _unitOfWork.Repository<FinancialAid>().DeleteAsync(financialAid);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("FinancialAid deleted successfully with ID: {Id}", id);

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting FinancialAid with ID: {Id}", id);
                throw;
            }
        }

        private GetFinancialAid MapToGetFinancialAid(FinancialAid financialAidEntity)
        {
            return new GetFinancialAid
            {
                Id = financialAidEntity.Id,
                ApprovalStatus = financialAidEntity.ApprovalStatus,
                percentage = financialAidEntity.percentage,
                ApplicationDate = financialAidEntity.ApplicationDate,
                PatientSSN = financialAidEntity.PatientSSN,
                OrganizationSSN = financialAidEntity.OrganizationSSN
            };
        }

        public async Task<IEnumerable<GetFinancialAid>> GetFinancialAidsByPatientAsync(Guid patientSsn)
        {
            try
            {
                var financialAids = await _unitOfWork.Repository<FinancialAid>()
                    .FindAsync(f => f.PatientSSN == patientSsn);

                return financialAids
                    .Select(MapToGetFinancialAid)
                    .OrderByDescending(f => f.ApplicationDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial aids for patient: {PatientSSN}", patientSsn);
                throw;
            }
        }

        public async Task<IEnumerable<GetFinancialAid>> GetFinancialAidsByOrganizationAsync(Guid organizationSsn)
        {
            try
            {
                var financialAids = await _unitOfWork.Repository<FinancialAid>()
                    .FindAsync(f => f.OrganizationSSN == organizationSsn);

                return financialAids
                    .Select(MapToGetFinancialAid)
                    .OrderByDescending(f => f.ApplicationDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial aids for organization: {OrganizationSSN}", organizationSsn);
                throw;
            }
        }

        public async Task<IEnumerable<GetFinancialAid>> GetFinancialAidsByStatusAsync(ApprovalStatus status)
        {
            try
            {
                var financialAids = await _unitOfWork.Repository<FinancialAid>()
                    .FindAsync(f => f.ApprovalStatus == status);

                return financialAids
                    .Select(MapToGetFinancialAid)
                    .OrderByDescending(f => f.ApplicationDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial aids by status: {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<GetFinancialAid>> GetPendingFinancialAidsAsync()
        {
            return await GetFinancialAidsByStatusAsync(ApprovalStatus.Pending);
        }
    }
}