using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Response;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class PermissionService : IPermissionService
{
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PermissionResponse>> GetAllAsync()
    {
        return await _context.Permissions
            .OrderBy(permission => permission.Key)
            .Select(permission => ToResponse(permission))
            .ToListAsync();
    }

    public async Task<IEnumerable<PermissionResponse>?> GetRolePermissionsAsync(long roleId)
    {
        var roleExists = await _context.Roles
            .AnyAsync(role => role.Id == roleId);

        if (!roleExists)
            return null;

        return await _context.RolePermissions
            .Where(rolePermission => rolePermission.RoleId == roleId)
            .OrderBy(rolePermission => rolePermission.Permission.Key)
            .Select(rolePermission => ToResponse(rolePermission.Permission))
            .ToListAsync();
    }

    private static PermissionResponse ToResponse(Permission permission)
    {
        return new PermissionResponse
        {
            Id = permission.Id,
            Key = permission.Key,
            Description = permission.Description
        };
    }
}
