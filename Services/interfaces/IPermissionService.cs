using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionResponse>> GetAllAsync();
    Task<IEnumerable<PermissionResponse>?> GetRolePermissionsAsync(long roleId);
}
