using AbleEaseCore.DTOs.PatientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbleEaseCore.DTOs.DisabilityDTOs.PateintDisabilityDto;

namespace AbleEaseCore.IServices
{
    public interface IPatientDisabilityService
    {
        Task AddDisabilityToPatientAsync(PatientDisabilityDto dto);
        Task AddPatientDisabilitiesAsync(Guid patientSSN, List<PatientDisabilityDto> disabilities);

        Task UpdatePatientDisabilityAsync(PatientDisabilityDto dto);

        Task RemoveDisabilityFromPatientAsync(Guid patientSSN, Guid disabilityId);
        Task RemoveAllDisabilitiesForPatientAsync(Guid patientSSN);

        Task<bool> PatientHasDisabilityAsync(Guid patientSSN, Guid disabilityId);

        Task<IEnumerable<PatientDisabilityDto>> GetDisabilitiesForPatientAsync(Guid patientSSN);
        Task<PatientDisabilityDto?> GetPatientDisabilityAsync(Guid patientSSN, Guid disabilityId);

        Task<IEnumerable<GetPatient>> GetPatientsByDisabilityAsync(Guid disabilityId);

        Task<(IEnumerable<PatientDisabilityDto> items, int totalCount)>
            GetPagedPatientDisabilitiesAsync(int pageNumber, int pageSize);
    }

}
