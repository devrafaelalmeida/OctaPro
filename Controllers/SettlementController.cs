using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.Authorization;
using OctaPro.DTO;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers
{
    [ApiController]
    [Authorize(Policy = Permissions.SettlementRead)]
    [Route("api/settlements")]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService _service;

        public SettlementController(ISettlementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SettlementResponse>>> GetAll([FromQuery] SettlementFilterRequest filter)
        {
            return Ok(await _service.GetAllAsync(filter));
        }

        [HttpGet("{idPublic:guid}")]
        public async Task<ActionResult<SettlementResponse>> GetById(Guid idPublic)
        {
            var settlement = await _service.GetByIdAsync(idPublic);
            if (settlement == null)
                return NotFound();

            return Ok(settlement);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.SettlementCreate)]
        public async Task<IActionResult> SaveSettlement(SettlementRequest request)
        {
            string? userLoggedUUID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userLoggedUUID == null)
            {
                return Unauthorized();
            }

            await _service.CreateAsync(request, Guid.Parse(userLoggedUUID));
            return StatusCode(201);
        }

        [HttpPost("{settlementId:guid}/add-installment")]
        [Authorize(Policy = Permissions.SettlementAddInstallment)]
        public async Task<ActionResult<InstallmentResponse>> AddInstallment(
            Guid settlementId,
            InstallmentRequest request)
        {
            var installment = await _service.AddInstallmentAsync(settlementId, request);

            return StatusCode(201, installment);
        }

        [HttpDelete("{settlementId:guid}")]
        [Authorize(Policy = Permissions.SettlementDelete)]
        public async Task<IActionResult> DeleteSettlement(Guid settlementId)
        {
            if (!await _service.DeleteAsync(settlementId))
                return NotFound("Acordo não encontrado.");

            return NoContent();
        }

        [HttpPut("{settlementId:guid}")]
        [Authorize(Policy = Permissions.SettlementUpdate)]
        public async Task<IActionResult> UpdateSettlement(Guid settlementId, SettlementRequest request)
        {
            if (!await _service.UpdateAsync(settlementId, request))
                return NotFound();

            return NoContent();
        }



    }
}
