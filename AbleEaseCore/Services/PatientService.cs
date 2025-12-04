using AbleEaseCore.DTOs.PatientDTOs;
using AbleEaseCore.IServices;
using AbleEaseDomain.Entities;
using AbleEaseDomain.IRepositeries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbleEaseCore.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IUnitOfWork unitOfWork, ILogger<PatientService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GetPatient> AddPatientAsync(AddPatient addPatient)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var ssn = Guid.NewGuid();

                // Validate RelativeSSN if provided
                if (addPatient.RelativeSSN.HasValue)
                {
                    var relativeExists = await _unitOfWork.Repository<Relative>()
                        .ExistsAsync(r => r.SSN == addPatient.RelativeSSN.Value);

                    if (!relativeExists)
                    {
                        throw new ArgumentException($"Relative with SSN {addPatient.RelativeSSN} not found");
                    }
                }

                // Create Patient (inherits from User, so no separate User creation needed)
                var patient = new Patient
                {
                    SSN = ssn,
                    Name = addPatient.Name,
                    Address = addPatient.Address,
                    ContactInfo = addPatient.ContactInfo,
                    Gender = addPatient.Gender,
                    BirthDate = addPatient.BirthDate,
                    RelativeSSN = addPatient.RelativeSSN
                };

                await _unitOfWork.Repository<Patient>().AddAsync(patient);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Patient created successfully with SSN: {SSN}", ssn);

                // Return the newly created patient with all related data
                return await GetPatientBySSNAsync(ssn)
                    ?? throw new InvalidOperationException("Failed to retrieve created patient");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating patient");
                throw;
            }
        }

        public async Task<GetPatient?> GetPatientBySSNAsync(Guid ssn)
        {
            try
            {
                // Use advanced includes with ThenInclude
                var patients = await _unitOfWork.Repository<Patient>()
                    .GetWithAdvancedIncludesAsync(
                        predicate: p => p.SSN == ssn,
                        includes: new Func<IQueryable<Patient>, IQueryable<Patient>>[]
                        {
                            q => q.Include(p => p.Relative),
                            q => q.Include(p => p.Caregiver),
                            q => q.Include(p => p.Program),
                            q => q.Include(p => p.MedicalInfo),
                            q => q.Include(p => p.reports)
                                  .ThenInclude(r => r.caregiver),
                            q => q.Include(p => p.financialAids)
                                  .ThenInclude(f => f.organization),
                            q => q.Include(p => p.Therapies)
                                  .ThenInclude(t => t.center),
                            q => q.Include(p => p.patientDisabilities)
                                  .ThenInclude(pd => pd.disability)
                        }
                    );

                var patientEntity = patients.FirstOrDefault();

                if (patientEntity == null)
                    return null;

                return MapToGetPatient(patientEntity, includeCollections: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with SSN: {SSN}", ssn);
                throw;
            }
        }

        public async Task<IEnumerable<GetPatient>> GetAllPatientsAsync()
        {
            try
            {
                // Get all patients with includes
                var patientEntities = await _unitOfWork.Repository<Patient>()
                    .GetWithAdvancedIncludesAsync(
                        predicate: null, // Get all
                        includes: new Func<IQueryable<Patient>, IQueryable<Patient>>[]
                        {
                            q => q.Include(p => p.Relative),
                            q => q.Include(p => p.Caregiver),
                            q => q.Include(p => p.Program),
                            q => q.Include(p => p.MedicalInfo),
                            q => q.Include(p => p.reports)
                                  .ThenInclude(r => r.caregiver),
                            q => q.Include(p => p.financialAids)
                                  .ThenInclude(f => f.organization),
                            q => q.Include(p => p.Therapies)
                                  .ThenInclude(t => t.center),
                            q => q.Include(p => p.patientDisabilities)
                                  .ThenInclude(pd => pd.disability)
                        }
                    );

                return patientEntities
                    .Select(p => MapToGetPatient(p, includeCollections: true))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public async Task<(IEnumerable<GetPatient> patients, int totalCount)> GetPagedPatientsAsync(
            int pageNumber, int pageSize)
        {
            try
            {
                // First get the paged patient IDs
                var (patients, totalCount) = await _unitOfWork.Repository<Patient>()
                    .GetPagedAsync(
                        pageNumber,
                        pageSize,
                        orderBy: q => q.OrderByDescending(p => p.BirthDate)
                    );

                var ssns = patients.Select(p => p.SSN).ToList();

                // Then load full data for those patients
                var fullPatients = await _unitOfWork.Repository<Patient>()
                    .GetWithAdvancedIncludesAsync(
                        predicate: p => ssns.Contains(p.SSN),
                        includes: new Func<IQueryable<Patient>, IQueryable<Patient>>[]
                        {
                            q => q.Include(p => p.Relative),
                            q => q.Include(p => p.Caregiver),
                            q => q.Include(p => p.Program),
                            q => q.Include(p => p.MedicalInfo),
                            q => q.Include(p => p.reports)
                                  .ThenInclude(r => r.caregiver),
                            q => q.Include(p => p.financialAids)
                                  .ThenInclude(f => f.organization),
                            q => q.Include(p => p.Therapies)
                                  .ThenInclude(t => t.center),
                            q => q.Include(p => p.patientDisabilities)
                                  .ThenInclude(pd => pd.disability)
                        }
                    );

                var result = fullPatients
                    .Select(p => MapToGetPatient(p, includeCollections: true))
                    .ToList();

                return (result, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged patients");
                throw;
            }
        }

        public async Task<GetPatient> UpdatePatientAsync(Guid ssn, UpdatePatient updatePatient)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var patient = await _unitOfWork.Repository<Patient>()
                    .FirstOrDefaultAsync(p => p.SSN == ssn);

                if (patient == null)
                    throw new KeyNotFoundException($"Patient with SSN {ssn} not found");

                // Validate Foreign Keys
                if (updatePatient.RelativeSSN.HasValue)
                {
                    var relativeExists = await _unitOfWork.Repository<Relative>()
                        .ExistsAsync(r => r.SSN == updatePatient.RelativeSSN.Value);
                    if (!relativeExists)
                        throw new ArgumentException($"Relative with SSN {updatePatient.RelativeSSN} not found");
                }

                if (updatePatient.CaregiverSSN.HasValue)
                {
                    var caregiverExists = await _unitOfWork.Repository<Caregiver>()
                        .ExistsAsync(c => c.SSN == updatePatient.CaregiverSSN.Value);
                    if (!caregiverExists)
                        throw new ArgumentException($"Caregiver with SSN {updatePatient.CaregiverSSN} not found");
                }

                if (updatePatient.ProgramOrganizationSSN.HasValue && updatePatient.ProgramId.HasValue)
                {
                    var programExists = await _unitOfWork.Repository<Program>()
                        .ExistsAsync(p => p.OrganizationSSN == updatePatient.ProgramOrganizationSSN.Value
                                       && p.Id == updatePatient.ProgramId.Value);
                    if (!programExists)
                        throw new ArgumentException("Program organization SSN or Program ID is invalid.");
                }
                else if (updatePatient.ProgramOrganizationSSN.HasValue != updatePatient.ProgramId.HasValue)
                {
                    throw new ArgumentException("Both ProgramOrganizationSSN and ProgramId must be provided together.");
                }

                // Apply updates
                if (!string.IsNullOrWhiteSpace(updatePatient.Name))
                    patient.Name = updatePatient.Name;
                if (!string.IsNullOrWhiteSpace(updatePatient.Address))
                    patient.Address = updatePatient.Address;
                if (!string.IsNullOrWhiteSpace(updatePatient.ContactInfo))
                    patient.ContactInfo = updatePatient.ContactInfo;
                if (updatePatient.Gender.HasValue)
                    patient.Gender = updatePatient.Gender.Value;
                if (updatePatient.BirthDate.HasValue)
                    patient.BirthDate = updatePatient.BirthDate.Value;

                patient.RelativeSSN = updatePatient.RelativeSSN;
                patient.CaregiverSSN = updatePatient.CaregiverSSN;
                patient.ProgramOrganizationSSN = updatePatient.ProgramOrganizationSSN;
                patient.ProgramId = updatePatient.ProgramId;

                await _unitOfWork.Repository<Patient>().UpdateAsync(patient);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Patient updated successfully with SSN: {SSN}", ssn);

                var updatedPatientDto = await GetPatientBySSNAsync(ssn);

                if (updatedPatientDto == null)
                {
                    throw new InvalidOperationException($"Failed to retrieve updated patient with SSN: {ssn}");
                }

                return updatedPatientDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating patient with SSN: {SSN}", ssn);
                throw;
            }
        }

        public async Task<bool> DeletePatientAsync(Guid ssn)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var patient = await _unitOfWork.Repository<Patient>()
                    .FirstOrDefaultAsync(p => p.SSN == ssn);

                if (patient == null)
                    return false;

                await _unitOfWork.Repository<Patient>().DeleteAsync(patient);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Patient deleted successfully with SSN: {SSN}", ssn);

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error deleting patient with SSN: {SSN}", ssn);
                throw;
            }
        }

        // ============================================
        // HELPER METHOD - MAP TO DTO
        // ============================================
        private GetPatient MapToGetPatient(Patient patientEntity, bool includeCollections = false)
        {
            var dto = new GetPatient
            {
                SSN = patientEntity.SSN,
                Name = patientEntity.Name,
                Address = patientEntity.Address,
                ContactInfo = patientEntity.ContactInfo,
                Gender = patientEntity.Gender,
                BirthDate = patientEntity.BirthDate,

                // Direct Relationships
                RelativeSSN = patientEntity.RelativeSSN,
                RelativeName = patientEntity.Relative?.Name,

                CaregiverSSN = patientEntity.CaregiverSSN,
                CaregiverName = patientEntity.Caregiver?.Name,

                ProgramOrganizationSSN = patientEntity.ProgramOrganizationSSN,
                ProgramId = patientEntity.ProgramId,
                ProgramName = patientEntity.Program?.Name
            };

            // Only include collections if explicitly requested
            if (includeCollections)
            {
            //    dto.MedicalInfo = patientEntity.MedicalInfo?
            //        .Select(mi => new GetMedicalInfo
            //        {
            //            Id = mi.Id,
            //            PatientSSN = mi.PatientSSN,
            //            RelativeSSN = mi.RelativeSSN,
            //            Description = mi.Description,
            //            DateAdded = mi.DateAdded
            //        })
            //        .ToList() ?? new List<GetMedicalInfo>();

            //    dto.Disabilities = patientEntity.patientDisabilities?
            //        .Where(pd => pd.disability != null)
            //        .Select(pd => new GetDisability
            //        {
            //            SSN = pd.disability!.SSN,
            //            Type = pd.disability.Type,
            //            Description = pd.disability.Description
            //        })
            //        .ToList() ?? new List<GetDisability>();

            //    dto.Reports = patientEntity.reports?
            //        .Select(r => new GetReport
            //        {
            //            Id = r.Id,
            //            Type = r.Type,
            //            Date = r.Date,
            //            CaregiverSSN = r.CaregiverSSN,
            //            CaregiverName = r.caregiver?.Name
            //        })
            //        .ToList() ?? new List<GetReport>();

            //    dto.FinancialAids = patientEntity.financialAids?
            //        .Select(fa => new GetFinancialAid
            //        {
            //            Id = fa.Id,
            //            Amount = fa.Amount,
            //            Status = fa.ApprovalStatus.ToString(),
            //            OrganizationSSN = fa.OrganizationSSN,
            //            OrganizationName = fa.organization?.Name
            //        })
            //        .ToList() ?? new List<GetFinancialAid>();

            //    dto.PhysicalTherapies = patientEntity.Therapies?
            //        .Select(pt => new GetPhysicalTherapy
            //        {
            //            Id = pt.Id,
            //            Type = pt.Type,
            //            Date = pt.Date,
            //            CenterID = pt.CenterID,
            //            CenterName = pt.center?.Name
            //        })
            //        .ToList() ?? new List<GetPhysicalTherapy>();
            }

            return dto;
        }
    }
}