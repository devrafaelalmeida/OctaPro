using OctaPro.DTO.Request;
using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionResponse>> GetAllAsync();
    Task<IEnumerable<PermissionResponse>?> GetDirectUserPermissionsAsync(Guid userIdPublic);
    Task<IEnumerable<EffectivePermissionResponse>?> GetEffectiveUserPermissionsAsync(Guid userIdPublic);
    Task<(bool Succeeded, string? Error, bool NotFound)> AssignUserPermissionAsync(Guid userIdPublic, UserPermissionRequest request);
    Task<(bool Succeeded, string? Error, bool NotFound)> RemoveUserPermissionAsync(Guid userIdPublic, int permissionId);
}
