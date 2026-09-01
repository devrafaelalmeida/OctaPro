using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.Authorization;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers;

[ApiController]
[Authorize(Policy = Permissions.CorporationRead)]
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
    [Authorize(Policy = Permissions.CorporationCreate)]
    public async Task<ActionResult<CorporationResponse>> Create(CorporationRequest request)
    {
        var corporation = await _service.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { idPublic = corporation.IdPublic }, corporation);
    }

    [HttpPut("{idPublic:guid}")]
    [Authorize(Policy = Permissions.CorporationUpdate)]
    public async Task<ActionResult<CorporationResponse>> Update(Guid idPublic, CorporationRequest request)
    {
        var corporation = await _service.UpdateAsync(idPublic, request);

        if (corporation == null)
            return NotFound();

        return Ok(corporation);
    }

    [HttpDelete("{idPublic:guid}")]
    [Authorize(Policy = Permissions.CorporationDelete)]
    public async Task<IActionResult> Delete(Guid idPublic)
    {
        if (!await _service.DeleteAsync(idPublic))
            return NotFound();

        return NoContent();
    }
}
