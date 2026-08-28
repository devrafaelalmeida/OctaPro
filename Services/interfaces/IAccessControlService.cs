using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Models;

namespace OctaPro.Services.interfaces;

public interface IAccessControlService
{
    Task<IReadOnlySet<string>> GetEffectivePermissionsAsync(User user);
    Task<IEnumerable<PermissionResponse>?> GetDirectUserPermissionsAsync(Guid userIdPublic);
    Task<IEnumerable<EffectivePermissionResponse>?> GetEffectiveUserPermissionsAsync(Guid userIdPublic);
    Task<bool> HasPermissionAsync(User user, string permission);
    Task<(bool Succeeded, string? Error, bool NotFound)> AssignUserPermissionAsync(Guid userIdPublic, UserPermissionRequest request);
    Task<(bool Succeeded, string? Error, bool NotFound)> RemoveUserPermissionAsync(Guid userIdPublic, int permissionId);
    Task<(bool Succeeded, string? Error, bool NotFound)> UpdateUserAccessControlAsync(
        Guid userIdPublic,
        AccessControlRequest request);
}
