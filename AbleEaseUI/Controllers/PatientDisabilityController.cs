using AbleEaseCore.IServices;
using AbleEaseCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AbleEaseCore.DTOs.DisabilityDTOs.PateintDisabilityDto;

namespace AbleEaseUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientDisabilityController : ControllerBase
    {
        private readonly IPatientDisabilityService _service;
        private readonly ILogger<PatientDisabilityController> _logger;

        public PatientDisabilityController(
            IPatientDisabilityService service,
            ILogger<PatientDisabilityController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ---------------------------------------------------------------------
        // ADD Disability to Patient
        // ---------------------------------------------------------------------
        [HttpPost("AddDisabilityToPatient")]
        public async Task<IActionResult> AddDisabilityToPatient([FromBody] PatientDisabilityDto dto)
        {
            _logger.LogInformation("Adding disability {DisabilityID} to patient {PatientSSN}",
                dto.DisabilityID, dto.PatientSSN);

            await _service.AddDisabilityToPatientAsync(dto);

            _logger.LogInformation("Disability added successfully.");

            return Ok(new
            {
                message = "Disability added to patient successfully.",
                data = dto
            });
        }

        // ---------------------------------------------------------------------
        // ADD Multiple Disabilities
        // ---------------------------------------------------------------------
        [HttpPost("AddRangeOfDisabilitiesToPatient/{patientSSN}")]
        public async Task<IActionResult> AddMultiple(Guid patientSSN, [FromBody] List<PatientDisabilityDto> list)
        {
            _logger.LogInformation("Adding bulk disabilities to patient {PatientSSN}", patientSSN);

            await _service.AddPatientDisabilitiesAsync(patientSSN, list);

            return Ok(new
            {
                message = "Multiple disabilities added successfully."
            });
        }

        // ---------------------------------------------------------------------
        // UPDATE
        // ---------------------------------------------------------------------
        [HttpPut("UpdatePatientDisability")]
        public async Task<IActionResult> UpdatePatientDisability([FromBody] PatientDisabilityDto dto)
        {
            _logger.LogInformation("Updating disability {DisabilityID} for patient {PatientSSN}",
                dto.DisabilityID, dto.PatientSSN);

            await _service.UpdatePatientDisabilityAsync(dto);

            return Ok(new
            {
                message = "Patient disability updated successfully."
            });
        }

        // ---------------------------------------------------------------------
        // DELETE ONE
        // ---------------------------------------------------------------------
        [HttpDelete("RemovePatientDisability/{patientSSN}/{disabilityId}")]
        public async Task<IActionResult> Remove(Guid patientSSN, Guid disabilityId)
        {
            _logger.LogInformation("Removing disability {DisabilityID} from patient {PatientSSN}",
                disabilityId, patientSSN);

            await _service.RemoveDisabilityFromPatientAsync(patientSSN, disabilityId);

            return Ok(new
            {
                message = "Disability removed from patient."
            });
        }

        // ---------------------------------------------------------------------
        // DELETE ALL for patient
        // ---------------------------------------------------------------------
        [HttpDelete("RemoveAllPatientDisabilities/{patientSSN}")]
        public async Task<IActionResult> RemoveAll(Guid patientSSN)
        {
            _logger.LogInformation("Removing ALL disabilities for patient {PatientSSN}", patientSSN);

            await _service.RemoveAllDisabilitiesForPatientAsync(patientSSN);

            return Ok(new
            {
                message = "All disabilities removed for patient."
            });
        }

        // ---------------------------------------------------------------------
        // GET Disabilities for Patient
        // ---------------------------------------------------------------------
        [HttpGet("GetPatientDisabilities/{patientSSN}")]
        public async Task<IActionResult> GetDisabilitiesForPatient(Guid patientSSN)
        {
            _logger.LogInformation("Getting disabilities for patient {PatientSSN}", patientSSN);

            var items = await _service.GetDisabilitiesForPatientAsync(patientSSN);

            return Ok(items);
        }

        // ---------------------------------------------------------------------
        // GET Single relation
        // ---------------------------------------------------------------------
        [HttpGet("GetPatientDisability/{patientSSN}/{disabilityId}")]
        public async Task<IActionResult> GetRelation(Guid patientSSN, Guid disabilityId)
        {
            _logger.LogInformation("Getting relation between patient {PatientSSN} and disability {DisabilityID}",
                patientSSN, disabilityId);

            var result = await _service.GetPatientDisabilityAsync(patientSSN, disabilityId);

            if (result == null)
                return NotFound("Relation not found.");

            return Ok(result);
        }

        // ---------------------------------------------------------------------
        // GET Patients who have specific disability
        // ---------------------------------------------------------------------
        [HttpGet("GetPatientsWithDisability/{disabilityId}")]
        public async Task<IActionResult> GetPatientsByDisability(Guid disabilityId)
        {
            _logger.LogInformation("Getting patients with disability {DisabilityID}", disabilityId);

            var items = await _service.GetPatientsByDisabilityAsync(disabilityId);

            return Ok(items);
        }

        // ---------------------------------------------------------------------
        // PAGINATION
        // ---------------------------------------------------------------------
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            if (pageNumber < 1 || pageSize < 1)
                return BadRequest(new { message = "Page number and page size must be greater than 0" });
            _logger.LogInformation("Getting paged patient-disability relations. Page: {Page}, Size: {Size}",
                pageNumber, pageSize);

           

            var (patients, totalCount) = await _service.GetPagedPatientDisabilitiesAsync(
                pageNumber, pageSize);

            return Ok(new
            {
                data = patients,
                pagination = new
                {
                    totalCount,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            });
        }
    }
}
