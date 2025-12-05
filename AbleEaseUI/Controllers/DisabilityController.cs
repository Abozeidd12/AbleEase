using AbleEaseCore.DTOs.DisabilityDTOs;
using AbleEaseCore.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbleEaseUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisabilityController : ControllerBase
    {

        private readonly IDisabilityService _DisabilityService;

        public DisabilityController(IDisabilityService DisabilityService)
        {
            _DisabilityService = DisabilityService;
        }

        [HttpPost("AddDisability")]
        public async Task<ActionResult<GetDisability>> AddDisability([FromBody] AddDisability dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _DisabilityService.AddDisabilityAsync(dto);
                return CreatedAtAction(nameof(GetDisability), new { id = result.SSN }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating the Disability" });
            }
        }

        [HttpGet("GetDisabilityByID/{id:guid}")]
        public async Task<ActionResult<GetDisability>> GetDisability(Guid ssn)
        {
            var Disability = await _DisabilityService.GetDisabilityByIDAsync(ssn);
            if (Disability == null)
                return NotFound(new { message = $"Disability with SSN {ssn} not found" });

            return Ok(Disability);
        }

        [HttpGet("GetAllDisabilities")]
        public async Task<ActionResult<IEnumerable<GetDisability>>> GetAllDisabilities()
        {
            var Disabilitys = await _DisabilityService.GetAllDisabilitiesAsync();
            return Ok(Disabilitys);
        }

        [HttpPut("UpdateDisability/{id:guid}")]
        public async Task<ActionResult<GetDisability>> UpdateDisability(
            Guid ssn, [FromBody] UpdateDisability dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _DisabilityService.UpdateDisabilityAsync(ssn, dto);
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
                return StatusCode(500, new { message = "An error occurred while updating the Disability" });
            }
        }

        [HttpDelete("DeleteDisability/{id:guid}")]
        public async Task<ActionResult> DeleteDisability(Guid ssn)
        {
            var result = await _DisabilityService.DeleteDisabilityAsync(ssn);
            if (!result)
                return NotFound(new { message = $"Disability with SSN {ssn} not found" });

            return NoContent();
        }

        [HttpGet("GetPagedDisabilities")]
        public async Task<ActionResult> GetPagedDisabilitys(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest(new { message = "Page number and page size must be greater than 0" });

            var (Disabilities, totalCount) = await _DisabilityService.GetPagedDisabilitiesAsync(
                pageNumber, pageSize);

            return Ok(new
            {
                data = Disabilities,
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
