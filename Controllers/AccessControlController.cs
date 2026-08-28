using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.Authorization;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers;

[ApiController]
[Authorize]
[Route("api/access-control")]
public class AccessControlController : ControllerBase
{
    private readonly IAccessControlService _service;

    public AccessControlController(IAccessControlService service)
    {
        _service = service;
    }

    [HttpGet("users/{userIdPublic:guid}")]
    [Authorize(Policy = Permissions.AccessControlRead)]
    public async Task<ActionResult<IEnumerable<EffectivePermissionResponse>>> GetUserAccessControl(Guid userIdPublic)
    {
        var permissions = await _service.GetEffectiveUserPermissionsAsync(userIdPublic);

        if (permissions == null)
            return NotFound("Usuário não encontrado.");

        return Ok(permissions);
    }

    [HttpPut("users/{userIdPublic:guid}")]
    [Authorize(Policy = Permissions.AccessControlUpdate)]
    public async Task<IActionResult> UpdateUserAccessControl(Guid userIdPublic, AccessControlRequest request)
    {
        var result = await _service.UpdateUserAccessControlAsync(userIdPublic, request);

        if (result.NotFound)
            return NotFound(result.Error);

        if (!result.Succeeded)
            return BadRequest(result.Error);

        return NoContent();
    }
}
