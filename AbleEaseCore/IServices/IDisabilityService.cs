using AbleEaseCore.DTOs.DisabilityDTOs;
using AbleEaseCore.DTOs.PatientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.IServices
{
    public interface IDisabilityService
    {

        Task<GetDisability> AddDisabilityAsync(AddDisability addDisability);
        Task<GetDisability?> GetDisabilityByIDAsync(Guid ssn);
        Task<IEnumerable<GetDisability>> GetAllDisabilitiesAsync();

        Task<GetDisability> UpdateDisabilityAsync(Guid ssn, UpdateDisability updateDisability);
        Task<bool> DeleteDisabilityAsync(Guid ssn);
        Task<(IEnumerable<GetDisability> Disabilities, int totalCount)> GetPagedDisabilitiesAsync(int pageNumber, int pageSize);


    }
}
