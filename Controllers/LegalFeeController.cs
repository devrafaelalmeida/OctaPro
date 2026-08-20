using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Manager,Common")]
    [Route("api/legal-fees")]
    public class LegalFeeController : ControllerBase
    {
        private readonly ILegalFeeService _service;

        public LegalFeeController(ILegalFeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegalFeeResponse>>> GetAll([FromQuery] SettlementFilterRequest filter)
        {
            return Ok(await _service.GetAllAsync(filter));
        }

        [HttpGet("{idPublic:guid}")]
        public async Task<ActionResult<LegalFeeResponse>> GetById(Guid idPublic)
        {
            var legalFee = await _service.GetByIdAsync(idPublic);
            if (legalFee == null)
                return NotFound();

            return Ok(legalFee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SaveLegalFee(LegalFeeRequest request)
        {
            string? userLoggedUUID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userLoggedUUID == null)
                return Unauthorized();

            await _service.CreateAsync(request, Guid.Parse(userLoggedUUID));
            return StatusCode(201);
        }

        [HttpPost("{legalFeeId:guid}/add-installment")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<InstallmentResponse>> AddInstallment(
            Guid legalFeeId,
            InstallmentRequest request)
        {
            var installment = await _service.AddInstallmentAsync(legalFeeId, request);

            return StatusCode(201, installment);
        }

        [HttpDelete("{legalFeeId:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteLegalFee(Guid legalFeeId)
        {
            if (!await _service.DeleteAsync(legalFeeId))
                return NotFound("Honorário não encontrado.");

            return NoContent();
        }

        [HttpPut("{legalFeeId:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateLegalFee(Guid legalFeeId, LegalFeeRequest request)
        {
            if (!await _service.UpdateAsync(legalFeeId, request))
                return NotFound();

            return NoContent();
        }
    }
}
