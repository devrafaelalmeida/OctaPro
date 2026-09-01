using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.Authorization;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers;

[ApiController]
[Authorize(Policy = Permissions.AccessControlRead)]
[Route("api/permissions")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;
    private readonly IAccessControlService _accessControlService;

    public PermissionController(
        IPermissionService permissionService,
        IAccessControlService accessControlService)
    {
        _permissionService = permissionService;
        _accessControlService = accessControlService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetAll()
    {
        return Ok(await _permissionService.GetAllAsync());
    }

    [HttpGet("roles/{roleId:long}")]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetRolePermissions(long roleId)
    {
        var permissions = await _permissionService.GetRolePermissionsAsync(roleId);

        if (permissions == null)
            return NotFound("Role não encontrada.");

        return Ok(permissions);
    }

    [HttpGet("users/{userIdPublic:guid}/direct")]
    public async Task<ActionResult<IEnumerable<PermissionResponse>>> GetDirectUserPermissions(Guid userIdPublic)
    {
        var permissions = await _accessControlService.GetDirectUserPermissionsAsync(userIdPublic);

        if (permissions == null)
            return NotFound("Usuário não encontrado.");

        return Ok(permissions);
    }

    [HttpGet("users/{userIdPublic:guid}/effective")]
    public async Task<ActionResult<IEnumerable<EffectivePermissionResponse>>> GetEffectiveUserPermissions(Guid userIdPublic)
    {
        var permissions = await _accessControlService.GetEffectiveUserPermissionsAsync(userIdPublic);

        if (permissions == null)
            return NotFound("Usuário não encontrado.");

        return Ok(permissions);
    }

    [HttpPost("users/{userIdPublic:guid}")]
    [Authorize(Policy = Permissions.AccessControlUpdate)]
    public async Task<IActionResult> AssignUserPermission(Guid userIdPublic, UserPermissionRequest request)
    {
        var result = await _accessControlService.AssignUserPermissionAsync(userIdPublic, request);

        if (result.NotFound)
            return NotFound(result.Error);

        if (!result.Succeeded)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("users/{userIdPublic:guid}/{permissionId:int}")]
    [Authorize(Policy = Permissions.AccessControlUpdate)]
    public async Task<IActionResult> RemoveUserPermission(Guid userIdPublic, int permissionId)
    {
        var result = await _accessControlService.RemoveUserPermissionAsync(userIdPublic, permissionId);

        if (result.NotFound)
            return NotFound(result.Error);

        if (!result.Succeeded)
            return BadRequest(result.Error);

        return NoContent();
    }
}
