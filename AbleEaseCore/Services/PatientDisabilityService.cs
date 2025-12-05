using AbleEaseCore.DTOs.PatientDTOs;
using AbleEaseCore.IServices;
using AbleEaseDomain.Entities;
using AbleEaseDomain.IRepositeries;
using Microsoft.Extensions.Logging;
using static AbleEaseCore.DTOs.DisabilityDTOs.PateintDisabilityDto;

namespace AbleEaseCore.Services
{
    public class PatientDisabilityService : IPatientDisabilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PatientDisabilityService> _logger;

        public PatientDisabilityService(IUnitOfWork unitOfWork, ILogger<PatientDisabilityService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // ========================================================
        // ADD
        // ========================================================
        public async Task AddDisabilityToPatientAsync(PatientDisabilityDto dto)
        {
            _logger.LogInformation("START AddDisabilityToPatient | DTO = {@dto}", dto);

            try
            {
                var exists = await _unitOfWork.Repository<PatientDisability>()
                    .ExistsAsync(x => x.PatientSSN == dto.PatientSSN
                                   && x.DisabilityID == dto.DisabilityID);

                if (exists)
                {
                    _logger.LogWarning("DISABILITY_EXISTS | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                        dto.PatientSSN, dto.DisabilityID);

                    throw new Exception("This disability already exists for this patient.");
                }

                var entity = new PatientDisability
                {
                    PatientSSN = dto.PatientSSN,
                    DisabilityID = dto.DisabilityID,
                    level = dto.Level,
                    notes = dto.Notes
                };

                await _unitOfWork.Repository<PatientDisability>().AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("SUCCESS AddDisabilityToPatient | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                    dto.PatientSSN, dto.DisabilityID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR AddDisabilityToPatient | DTO={@dto}", dto);
                throw;
            }
        }

        public async Task AddPatientDisabilitiesAsync(Guid patientSSN, List<PatientDisabilityDto> list)
        {
            _logger.LogInformation("START AddPatientDisabilities | PatientSSN={PatientSSN} | Count={Count}",
                patientSSN, list.Count);

            try
            {
                foreach (var dto in list)
                {
                    var entity = new PatientDisability
                    {
                        PatientSSN = patientSSN,
                        DisabilityID = dto.DisabilityID,
                        level = dto.Level,
                        notes = dto.Notes
                    };

                    await _unitOfWork.Repository<PatientDisability>().AddAsync(entity);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("SUCCESS AddPatientDisabilities | PatientSSN={PatientSSN}", patientSSN);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR AddPatientDisabilities | PatientSSN={PatientSSN}", patientSSN);
                throw;
            }
        }

        // ========================================================
        // UPDATE
        // ========================================================
        public async Task UpdatePatientDisabilityAsync(PatientDisabilityDto dto)
        {
            _logger.LogInformation("START UpdatePatientDisability | DTO={@dto}", dto);

            try
            {
                var repo = _unitOfWork.Repository<PatientDisability>();

                var entity = await repo.FirstOrDefaultAsync(
                    x => x.PatientSSN == dto.PatientSSN && x.DisabilityID == dto.DisabilityID);

                if (entity == null)
                {
                    _logger.LogWarning("NOT FOUND UpdatePatientDisability | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                        dto.PatientSSN, dto.DisabilityID);

                    throw new Exception("Relation not found.");
                }

                entity.level = dto.Level;
                entity.notes = dto.Notes;

                await repo.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("SUCCESS UpdatePatientDisability | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                    dto.PatientSSN, dto.DisabilityID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR UpdatePatientDisability | DTO={@dto}", dto);
                throw;
            }
        }

        // ========================================================
        // DELETE
        // ========================================================
        public async Task RemoveDisabilityFromPatientAsync(Guid patientSSN, Guid disabilityId)
        {
            _logger.LogInformation("START RemoveDisabilityFromPatient | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                patientSSN, disabilityId);

            try
            {
                var repo = _unitOfWork.Repository<PatientDisability>();

                var entity = await repo.FirstOrDefaultAsync(
                    x => x.PatientSSN == patientSSN && x.DisabilityID == disabilityId);

                if (entity == null)
                {
                    _logger.LogWarning("NOT FOUND RemoveDisabilityFromPatient | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                        patientSSN, disabilityId);
                    return;
                }

                await repo.DeleteAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("SUCCESS RemoveDisabilityFromPatient | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                    patientSSN, disabilityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR RemoveDisabilityFromPatient | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                    patientSSN, disabilityId);
                throw;
            }
        }

        public async Task RemoveAllDisabilitiesForPatientAsync(Guid patientSSN)
        {
            _logger.LogInformation("START RemoveAllDisabilitiesForPatient | PatientSSN={PatientSSN}", patientSSN);

            try
            {
                var repo = _unitOfWork.Repository<PatientDisability>();

                var items = await repo.FindAsync(x => x.PatientSSN == patientSSN);

                await repo.DeleteRangeAsync(items);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("SUCCESS RemoveAllDisabilitiesForPatient | PatientSSN={PatientSSN}", patientSSN);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR RemoveAllDisabilitiesForPatient | PatientSSN={PatientSSN}", patientSSN);
                throw;
            }
        }

        // ========================================================
        // CHECK
        // ========================================================
        public async Task<bool> PatientHasDisabilityAsync(Guid patientSSN, Guid disabilityId)
        {
            _logger.LogInformation("CHECK PatientHasDisability | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                patientSSN, disabilityId);

            try
            {
                return await _unitOfWork.Repository<PatientDisability>()
                    .ExistsAsync(x => x.PatientSSN == patientSSN && x.DisabilityID == disabilityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR PatientHasDisability | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                    patientSSN, disabilityId);
                throw;
            }
        }

        // ========================================================
        // READ
        // ========================================================
        public async Task<IEnumerable<PatientDisabilityDto>> GetDisabilitiesForPatientAsync(Guid patientSSN)
        {
            _logger.LogInformation("START GetDisabilitiesForPatient | PatientSSN={PatientSSN}", patientSSN);

            try
            {
                var repo = _unitOfWork.Repository<PatientDisability>();

                var items = await repo.FindIncludingAsync(
                    x => x.PatientSSN == patientSSN,
                    x => x.disability
                );

                var result = items.Select(x => new PatientDisabilityDto
                {
                    PatientSSN = x.PatientSSN,
                    DisabilityID = x.DisabilityID,
                    Level = x.level,
                    Notes = x.notes,
                    DisabilityName = x.disability?.name
                });

                _logger.LogInformation("SUCCESS GetDisabilitiesForPatient | Count={Count}", result.Count());

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR GetDisabilitiesForPatient | PatientSSN={PatientSSN}", patientSSN);
                throw;
            }
        }

        public async Task<PatientDisabilityDto?> GetPatientDisabilityAsync(Guid patientSSN, Guid disabilityId)
        {
            _logger.LogInformation("START GetPatientDisability | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                patientSSN, disabilityId);

            try
            {
                var repo = _unitOfWork.Repository<PatientDisability>();

                var x = await repo.FirstOrDefaultAsync(
                    p => p.PatientSSN == patientSSN && p.DisabilityID == disabilityId);

                if (x == null)
                {
                    _logger.LogWarning("NOT FOUND GetPatientDisability | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                        patientSSN, disabilityId);

                    return null;
                }

                return new PatientDisabilityDto
                {
                    PatientSSN = x.PatientSSN,
                    DisabilityID = x.DisabilityID,
                    Level = x.level,
                    Notes = x.notes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR GetPatientDisability | PatientSSN={PatientSSN} | DisabilityID={DisabilityID}",
                    patientSSN, disabilityId);
                throw;
            }
        }

        public async Task<IEnumerable<GetPatient>> GetPatientsByDisabilityAsync(Guid disabilityId)
        {
            _logger.LogInformation("START GetPatientsByDisability | DisabilityID={DisabilityID}", disabilityId);

            try
            {
                var repo = _unitOfWork.Repository<PatientDisability>();

                var items = await repo.FindIncludingAsync(
                    x => x.DisabilityID == disabilityId,
                    x => x.patient
                );

                var result = items.Select(x => new GetPatient
                {
                    SSN = x.patient!.SSN,
                    Name = x.patient!.Name,
                    Gender = x.patient!.Gender,
                    BirthDate = x.patient!.BirthDate
                });

                _logger.LogInformation("SUCCESS GetPatientsByDisability | Count={Count}", result.Count());

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR GetPatientsByDisability | DisabilityID={DisabilityID}", disabilityId);
                throw;
            }
        }

        // ========================================================
        // PAGINATION
        // ========================================================
        public async Task<(IEnumerable<PatientDisabilityDto> items, int totalCount)>
            GetPagedPatientDisabilitiesAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("START GetPagedPatientDisabilities | Page={Page} | Size={Size}",
                pageNumber, pageSize);

            try
            {
                var repo = _unitOfWork.Repository<PatientDisability>();

                var result = await repo.GetPagedAsync(pageNumber, pageSize);

                var items = result.items.Select(x => new PatientDisabilityDto
                {
                    PatientSSN = x.PatientSSN,
                    DisabilityID = x.DisabilityID,
                    Level = x.level,
                    Notes = x.notes
                });

                _logger.LogInformation("SUCCESS GetPagedPatientDisabilities | Count={Count}", items.Count());

                return (items, result.totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR GetPagedPatientDisabilities");
                throw;
            }
        }
    }
}
