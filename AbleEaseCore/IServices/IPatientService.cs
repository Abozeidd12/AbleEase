using AbleEaseCore.DTOs.PatientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbleEaseCore.IServices
{
    public interface IPatientService
    {
        Task<GetPatient> AddPatientAsync(AddPatient addPatient);
        Task<GetPatient?> GetPatientBySSNAsync(Guid ssn);
        Task<IEnumerable<GetPatient>> GetAllPatientsAsync();
        Task<GetPatient> UpdatePatientAsync(Guid ssn, UpdatePatient updatePatient);
        Task<bool> DeletePatientAsync(Guid ssn);
        Task<(IEnumerable<GetPatient> patients, int totalCount)> GetPagedPatientsAsync(int pageNumber, int pageSize);

    }
}
