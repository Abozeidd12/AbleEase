using AbleEaseCore.DTOs.PatientDTOs;
using AbleEaseCore.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbleEaseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("AddPatient")]
        public async Task<ActionResult<GetPatient>> AddPatient([FromBody] AddPatient dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _patientService.AddPatientAsync(dto);
                return CreatedAtAction(nameof(GetPatient), new { ssn = result.SSN }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating the patient" });
            }
        }

        [HttpGet("GetPatientBySSN/{ssn:guid}")]
        public async Task<ActionResult<GetPatient>> GetPatient(Guid ssn)
        {
            var patient = await _patientService.GetPatientBySSNAsync(ssn);
            if (patient == null)
                return NotFound(new { message = $"Patient with SSN {ssn} not found" });

            return Ok(patient);
        }

        [HttpGet("GetAllPatients")]
        public async Task<ActionResult<IEnumerable<GetPatient>>> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpPut("UpdatePatient/{ssn:guid}")]
        public async Task<ActionResult<GetPatient>> UpdatePatient(
            Guid ssn, [FromBody] UpdatePatient dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _patientService.UpdatePatientAsync(ssn, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the patient" });
            }
        }

        [HttpDelete("DeletePatient/{ssn:guid}")]
        public async Task<ActionResult> DeletePatient(Guid ssn)
        {
            var result = await _patientService.DeletePatientAsync(ssn);
            if (!result)
                return NotFound(new { message = $"Patient with SSN {ssn} not found" });

            return NoContent();
        }

        [HttpGet("GetPagedPatients")]
        public async Task<ActionResult> GetPagedPatients(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest(new { message = "Page number and page size must be greater than 0" });

            var (patients, totalCount) = await _patientService.GetPagedPatientsAsync(
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