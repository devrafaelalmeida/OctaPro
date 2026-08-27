using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Manager")]
[Route("api/permissions")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _service;

    public PermissionController(IPermissionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("users/{userIdPublic:guid}/direct")]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetDirectUserPermissions(Guid userIdPublic)
    {
        var permissions = await _service.GetDirectUserPermissionsAsync(userIdPublic);

        if (permissions == null)
            return NotFound("Usuário não encontrado.");

        return Ok(permissions);
    }

    [HttpGet("users/{userIdPublic:guid}/effective")]
    public async Task<ActionResult<IEnumerable<EffectivePermissionResponse>>> GetEffectiveUserPermissions(Guid userIdPublic)
    {
        var permissions = await _service.GetEffectiveUserPermissionsAsync(userIdPublic);

        if (permissions == null)
            return NotFound("Usuário não encontrado.");

        return Ok(permissions);
    }

    [HttpPost("users/{userIdPublic:guid}")]
    public async Task<IActionResult> AssignUserPermission(Guid userIdPublic, UserPermissionRequest request)
    {
        var result = await _service.AssignUserPermissionAsync(userIdPublic, request);

        if (result.NotFound)
            return NotFound(result.Error);

        if (!result.Succeeded)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("users/{userIdPublic:guid}/{permissionId:int}")]
    public async Task<IActionResult> RemoveUserPermission(Guid userIdPublic, int permissionId)
    {
        var result = await _service.RemoveUserPermissionAsync(userIdPublic, permissionId);

        if (result.NotFound)
            return NotFound(result.Error);

        if (!result.Succeeded)
            return BadRequest(result.Error);

        return NoContent();
    }
}
