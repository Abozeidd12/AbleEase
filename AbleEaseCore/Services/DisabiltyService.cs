using AbleEaseCore.DTOs.DisabilityDTOs;
using AbleEaseCore.IServices;
using AbleEaseDomain.Entities;
using AbleEaseDomain.IRepositeries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.Services
{
    public class DisabilityService : IDisabilityService
    {




        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DisabilityService> _logger;

        public DisabilityService(IUnitOfWork unitOfWork, ILogger<DisabilityService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GetDisability> AddDisabilityAsync(AddDisability addDisability)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var ssn = Guid.NewGuid();
                

                // Create Disability (inherits from User, so no separate User creation needed)
                var Disability = new Disability
                {
                    SSN = ssn,
                    type = addDisability.type,
                    description = addDisability.description,
                    name = addDisability.name
                    
                };

                await _unitOfWork.Repository<Disability>().AddAsync(Disability);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Disability created successfully with SSN: {SSN}", ssn);

                // Return the newly created Disability with all related data
                return await GetDisabilityByIDAsync(ssn)
                    ?? throw new InvalidOperationException("Failed to retrieve created Disability");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating Disability");
                throw;
            }
        }

        public async Task<GetDisability?> GetDisabilityByIDAsync(Guid ssn)
        {
            try
            {
                // Use advanced includes with ThenInclude
                var DisabilityEntity = await _unitOfWork.Repository<Disability>()
                    .GetByIdAsync(ssn);


                if (DisabilityEntity == null)
                    return null;

                return MapToGetDisability(DisabilityEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Disability with SSN: {SSN}", ssn);
                throw;
            }
        }

        public async Task<IEnumerable<GetDisability>> GetAllDisabilitiesAsync()
        {
            try
            {
                // Get all Disabilitys with includes
                var DisabilityEntities = await _unitOfWork.Repository<Disability>()
                   .GetAllAsync();

                return DisabilityEntities
                    .Select(d => MapToGetDisability(d))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Disabilitys");
                throw;
            }
        }

        public async Task<(IEnumerable<GetDisability> Disabilities, int totalCount)> GetPagedDisabilitiesAsync(
            int pageNumber, int pageSize)
        {
            try
            {
                // First get the paged Disability IDs
                var (Disabilities, totalCount) = await _unitOfWork.Repository<Disability>()
                    .GetPagedAsync(
                        pageNumber,
                        pageSize
                       
                    );

                var ssns = Disabilities.Select(p => p.SSN).ToList();

                // Then load full data for those Disabilitys
                var fullDisabilitys = await _unitOfWork.Repository<Disability>()
                    .GetWithAdvancedIncludesAsync(p => ssns.Contains(p.SSN));

                var result = fullDisabilitys
                    .Select(d => MapToGetDisability(d))
                    .ToList();

                return (result, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged Disabilitys");
                throw;
            }
        }

        public async Task<GetDisability> UpdateDisabilityAsync(Guid ssn, UpdateDisability updateDisability)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var Disability = await _unitOfWork.Repository<Disability>()
                    .FirstOrDefaultAsync(p => p.SSN == ssn);

                if (Disability == null)
                    throw new KeyNotFoundException($"Disability with SSN {ssn} not found");

              

               

                
               

                // Apply updates
                if (!string.IsNullOrWhiteSpace(updateDisability.name))
                    Disability.name = updateDisability.name;
                if (!string.IsNullOrWhiteSpace(updateDisability.type))
                    Disability.type = updateDisability.type;
                if (!string.IsNullOrWhiteSpace(updateDisability.description))
                    Disability.description = updateDisability.description;
               

                
                await _unitOfWork.Repository<Disability>().UpdateAsync(Disability);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Disability updated successfully with SSN: {SSN}", ssn);

                var updatedDisabilityDto = await GetDisabilityByIDAsync(ssn);

                if (updatedDisabilityDto == null)
                {
                    throw new InvalidOperationException($"Failed to retrieve updated Disability with SSN: {ssn}");
                }

                return updatedDisabilityDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating Disability with SSN: {SSN}", ssn);
                throw;
            }
        }

        public async Task<bool> DeleteDisabilityAsync(Guid ssn)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var Disability = await _unitOfWork.Repository<Disability>()
                    .FirstOrDefaultAsync(p => p.SSN == ssn);

                if (Disability == null)
                    return false;

                await _unitOfWork.Repository<Disability>().DeleteAsync(Disability);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Disability deleted successfully with SSN: {SSN}", ssn);

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting Disability with SSN: {SSN}", ssn);
                throw;
            }
        }


        private GetDisability MapToGetDisability(Disability DisabilityEntity, bool includeCollections = false)
        {
            var dto = new GetDisability
            {
                SSN = DisabilityEntity.SSN,
                name = DisabilityEntity.name,
                type = DisabilityEntity.type,
                description = DisabilityEntity.description,
                
            };
                    
            return dto;
        }

    }
}
