using OctaPro.Models;

namespace OctaPro.Services.interfaces;

public interface IAccessControlService
{
    Task<IReadOnlySet<string>> GetEffectivePermissionsAsync(User user);
    Task<bool> HasPermissionAsync(User user, string permission);
}
