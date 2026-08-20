using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Manager,Common")]
[Route("api/corporations")]
public class CorporationController : ControllerBase
{
    private readonly ICorporationService _service;

    public CorporationController(ICorporationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CorporationResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{idPublic:guid}")]
    public async Task<ActionResult<CorporationResponse>> GetById(Guid idPublic)
    {
        var corporation = await _service.GetByIdAsync(idPublic);

        if (corporation == null)
            return NotFound();

        return Ok(corporation);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<CorporationResponse>> Create(CorporationRequest request)
    {
        var corporation = await _service.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { idPublic = corporation.IdPublic }, corporation);
    }

    [HttpPut("{idPublic:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<CorporationResponse>> Update(Guid idPublic, CorporationRequest request)
    {
        var corporation = await _service.UpdateAsync(idPublic, request);

        if (corporation == null)
            return NotFound();

        return Ok(corporation);
    }

    [HttpDelete("{idPublic:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid idPublic)
    {
        if (!await _service.DeleteAsync(idPublic))
            return NotFound();

        return NoContent();
    }
}
