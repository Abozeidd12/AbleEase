using AbleEaseCore.DTOs.FinancialAidDTOs;
using AbleEaseCore.IServices;
using AbleEaseDomain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AbleEaseUI.Controllers
{
    public class FinancialAidController : ControllerBase
    {
        private readonly IFinancialAidService _financialAidService;

        public FinancialAidController(IFinancialAidService financialAidService)
        {
            _financialAidService = financialAidService;
        }

        [HttpPost("AddFinancialAid")]
        public async Task<ActionResult<GetFinancialAid>> AddFinancialAid([FromBody] AddFinancialAid dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _financialAidService.AddFinancialAidAsync(dto);
                return CreatedAtAction(
                    nameof(GetFinancialAid),
                    new { id = result.Id },
                    result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the financial aid application",
                    details = ex.Message
                });
            }
        }

        [HttpGet("GetFinancialAid/{id:guid}")]
        public async Task<ActionResult<GetFinancialAid>> GetFinancialAid(Guid id)
        {
            var financialAid = await _financialAidService.GetFinancialAidByIDAsync(id);

            if (financialAid == null)
                return NotFound(new { message = $"Financial aid with ID {id} not found" });

            return Ok(financialAid);
        }

        [HttpGet("GetAllFinancialAids")]
        public async Task<ActionResult<IEnumerable<GetFinancialAid>>> GetAllFinancialAids()
        {
            try
            {
                var financialAids = await _financialAidService.GetAllFinancialAidsAsync();
                return Ok(financialAids);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while retrieving financial aids",
                    details = ex.Message
                });
            }
        }

        [HttpGet("GetPagedFinancialAids")]
        public async Task<ActionResult> GetPagedFinancialAids(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest(new { message = "Page number and page size must be greater than 0" });

            if (pageSize > 100)
                return BadRequest(new { message = "Page size cannot exceed 100" });

            try
            {
                var (financialAids, totalCount) = await _financialAidService
                    .GetPagedFinancialAidsAsync(pageNumber, pageSize);

                return Ok(new
                {
                    data = financialAids,
                    pagination = new
                    {
                        totalCount,
                        pageNumber,
                        pageSize,
                        totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                        hasNextPage = pageNumber * pageSize < totalCount,
                        hasPreviousPage = pageNumber > 1
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while retrieving financial aids",
                    details = ex.Message
                });
            }
        }

        [HttpPut("UpdateFinancialAid/{id:guid}")]
        public async Task<ActionResult<GetFinancialAid>> UpdateFinancialAid(
            Guid id,
            [FromBody] UpdateFinancialAid dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _financialAidService.UpdateFinancialAidAsync(id, dto);
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the financial aid",
                    details = ex.Message
                });
            }
        }

        [HttpDelete("DeleteFinancialAid/{id:guid}")]
        public async Task<ActionResult> DeleteFinancialAid(Guid id)
        {
            try
            {
                var result = await _financialAidService.DeleteFinancialAidAsync(id);

                if (!result)
                    return NotFound(new { message = $"Financial aid with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while deleting the financial aid",
                    details = ex.Message
                });
            }
        }

        [HttpGet("GetFinancialAidsByPatient/{patientSsn:guid}")]
        public async Task<ActionResult<IEnumerable<GetFinancialAid>>> GetFinancialAidsByPatient(Guid patientSsn)
        {
            try
            {
                var financialAids = await _financialAidService.GetFinancialAidsByPatientAsync(patientSsn);
                return Ok(financialAids);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving financial aids" });
            }
        }

        [HttpGet("GetFinancialAidsByOrganization/{organizationSsn:guid}")]
        public async Task<ActionResult<IEnumerable<GetFinancialAid>>> GetFinancialAidsByOrganization(Guid organizationSsn)
        {
            try
            {
                var financialAids = await _financialAidService.GetFinancialAidsByOrganizationAsync(organizationSsn);
                return Ok(financialAids);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving financial aids" });
            }
        }

        [HttpGet("GetFinancialAidsByStatus/{status}")]
        public async Task<ActionResult<IEnumerable<GetFinancialAid>>> GetFinancialAidsByStatus(string status)
        {
            try
            {
                if (!Enum.TryParse<ApprovalStatus>(status, true, out var approvalStatus))
                {
                    return BadRequest(new { message = $"Invalid approval status. Valid values are: {string.Join(", ", Enum.GetNames(typeof(ApprovalStatus)))}" });
                }

                var financialAids = await _financialAidService.GetFinancialAidsByStatusAsync(approvalStatus);
                return Ok(financialAids);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving financial aids" });
            }
        }

        [HttpGet("GetPendingFinancialAids")]
        public async Task<ActionResult<IEnumerable<GetFinancialAid>>> GetPendingFinancialAids()
        {
            try
            {
                var financialAids = await _financialAidService.GetPendingFinancialAidsAsync();
                return Ok(financialAids);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving pending financial aids" });
            }
        }

    }
}
