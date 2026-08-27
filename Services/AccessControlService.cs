using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class AccessControlService : IAccessControlService
{
    private readonly AppDbContext _context;

    public AccessControlService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlySet<string>> GetEffectivePermissionsAsync(User user)
    {
        var rolePermissions = await _context.UserRoles
            .Where(userRole => userRole.UserId == user.Id)
            .Join(
                _context.RolePermissions,
                userRole => userRole.RoleId,
                rolePermission => rolePermission.RoleId,
                (_, rolePermission) => rolePermission.Permission.Key)
            .ToListAsync();

        var userPermissions = await _context.UserPermissions
            .Where(userPermission => userPermission.UserId == user.Id)
            .Select(userPermission => userPermission.Permission.Key)
            .ToListAsync();

        return rolePermissions
            .Concat(userPermissions)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<bool> HasPermissionAsync(User user, string permission)
    {
        var permissions = await GetEffectivePermissionsAsync(user);
        return permissions.Contains(permission);
    }
}
