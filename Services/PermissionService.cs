using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class PermissionService : IPermissionService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public PermissionService(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<PermissionResponse>> GetAllAsync()
    {
        return await _context.Permissions
            .OrderBy(permission => permission.Key)
            .Select(permission => ToResponse(permission))
            .ToListAsync();
    }

    public async Task<IEnumerable<PermissionResponse>?> GetDirectUserPermissionsAsync(Guid userIdPublic)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return null;

        return await _context.UserPermissions
            .Where(userPermission => userPermission.UserId == user.Id)
            .OrderBy(userPermission => userPermission.Permission.Key)
            .Select(userPermission => ToResponse(userPermission.Permission))
            .ToListAsync();
    }

    public async Task<IEnumerable<EffectivePermissionResponse>?> GetEffectiveUserPermissionsAsync(Guid userIdPublic)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return null;

        var rolePermissions = await _context.UserRoles
            .Where(userRole => userRole.UserId == user.Id)
            .Join(
                _context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new { userRole.RoleId, RoleName = role.Name ?? string.Empty })
            .Join(
                _context.RolePermissions.Include(rolePermission => rolePermission.Permission),
                userRole => userRole.RoleId,
                rolePermission => rolePermission.RoleId,
                (userRole, rolePermission) => new
                {
                    Permission = rolePermission.Permission,
                    userRole.RoleName
                })
            .ToListAsync();

        var directPermissions = await _context.UserPermissions
            .Where(userPermission => userPermission.UserId == user.Id)
            .Include(userPermission => userPermission.Permission)
            .ToListAsync();

        return rolePermissions
            .Select(rolePermission => new
            {
                rolePermission.Permission.Id,
                rolePermission.Permission.Key,
                rolePermission.Permission.Description,
                FromRole = true,
                Direct = false,
                Role = rolePermission.RoleName
            })
            .Concat(directPermissions.Select(userPermission => new
            {
                userPermission.Permission.Id,
                userPermission.Permission.Key,
                userPermission.Permission.Description,
                FromRole = false,
                Direct = true,
                Role = string.Empty
            }))
            .GroupBy(permission => permission.Id)
            .Select(group => new EffectivePermissionResponse
            {
                Id = group.Key,
                Key = group.First().Key,
                Description = group.First().Description,
                FromRole = group.Any(permission => permission.FromRole),
                Direct = group.Any(permission => permission.Direct),
                Roles = group
                    .Where(permission => permission.FromRole && !string.IsNullOrWhiteSpace(permission.Role))
                    .Select(permission => permission.Role)
                    .Distinct()
                    .OrderBy(role => role)
                    .ToList()
            })
            .OrderBy(permission => permission.Key)
            .ToList();
    }

    public async Task<(bool Succeeded, string? Error, bool NotFound)> AssignUserPermissionAsync(
        Guid userIdPublic,
        UserPermissionRequest request)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return (false, "Usuário não encontrado.", true);

        var permissionExists = await _context.Permissions
            .AnyAsync(permission => permission.Id == request.PermissionId);

        if (!permissionExists)
            return (false, "Permissão não encontrada.", true);

        var alreadyAssigned = await _context.UserPermissions
            .AnyAsync(userPermission =>
                userPermission.UserId == user.Id &&
                userPermission.PermissionId == request.PermissionId);

        if (alreadyAssigned)
            return (true, null, false);

        _context.UserPermissions.Add(new UserPermission
        {
            UserId = user.Id,
            PermissionId = request.PermissionId
        });

        await _context.SaveChangesAsync();

        return (true, null, false);
    }

    public async Task<(bool Succeeded, string? Error, bool NotFound)> RemoveUserPermissionAsync(
        Guid userIdPublic,
        int permissionId)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return (false, "Usuário não encontrado.", true);

        var userPermission = await _context.UserPermissions
            .FirstOrDefaultAsync(permission =>
                permission.UserId == user.Id &&
                permission.PermissionId == permissionId);

        if (userPermission == null)
            return (false, "Permissão direta não encontrada para este usuário.", true);

        _context.UserPermissions.Remove(userPermission);
        await _context.SaveChangesAsync();

        return (true, null, false);
    }

    private async Task<User?> FindManageableUserAsync(Guid userIdPublic)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        return await _context.Users
            .FirstOrDefaultAsync(user =>
                user.IdPublic == userIdPublic &&
                user.CorporationId == currentUser.CorporationId);
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
