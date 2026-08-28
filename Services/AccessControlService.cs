using Microsoft.EntityFrameworkCore;
using OctaPro.Authorization;
using OctaPro.Data;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Enums;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class AccessControlService : IAccessControlService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AccessControlService(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
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

    public async Task<IEnumerable<PermissionResponse>?> GetDirectUserPermissionsAsync(Guid userIdPublic)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return null;

        return await _context.UserPermissions
            .Where(userPermission => userPermission.UserId == user.Id)
            .OrderBy(userPermission => userPermission.Permission.Key)
            .Select(userPermission => ToPermissionResponse(userPermission.Permission))
            .ToListAsync();
    }

    public async Task<IEnumerable<EffectivePermissionResponse>?> GetEffectiveUserPermissionsAsync(Guid userIdPublic)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return null;

        var rolePermissions = await GetRolePermissionsForUserAsync(user.Id);
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

    public async Task<bool> HasPermissionAsync(User user, string permission)
    {
        var permissions = await GetEffectivePermissionsAsync(user);
        return permissions.Contains(permission);
    }

    public async Task<(bool Succeeded, string? Error, bool NotFound)> AssignUserPermissionAsync(
        Guid userIdPublic,
        UserPermissionRequest request)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return (false, "Usuário não encontrado.", true);

        var validation = await ValidateExtraPermissionsAsync([request.PermissionId]);

        if (!validation.Succeeded)
            return validation;

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

    public async Task<(bool Succeeded, string? Error, bool NotFound)> UpdateUserAccessControlAsync(
        Guid userIdPublic,
        AccessControlRequest request)
    {
        var user = await FindManageableUserAsync(userIdPublic);

        if (user == null)
            return (false, "Usuário não encontrado.", true);

        var rolesValidation = await ValidateRolePermissionsAsync(request.RolesPermissions);

        if (!rolesValidation.Succeeded)
            return rolesValidation;

        var extrasValidation = await ValidateExtraPermissionsAsync(request.ExtrasPermissions);

        if (!extrasValidation.Succeeded)
            return extrasValidation;

        var requestedExtraPermissionIds = request.ExtrasPermissions
            .Distinct()
            .ToHashSet();

        var currentUserPermissions = await _context.UserPermissions
            .Where(userPermission => userPermission.UserId == user.Id)
            .ToListAsync();

        var currentExtraPermissionIds = currentUserPermissions
            .Select(userPermission => userPermission.PermissionId)
            .ToHashSet();

        var permissionsToAdd = requestedExtraPermissionIds
            .Except(currentExtraPermissionIds)
            .ToArray();

        var permissionsToRemove = currentUserPermissions
            .Where(userPermission => !requestedExtraPermissionIds.Contains(userPermission.PermissionId))
            .ToList();

        foreach (var permissionId in permissionsToAdd)
        {
            _context.UserPermissions.Add(new UserPermission
            {
                UserId = user.Id,
                PermissionId = permissionId
            });
        }

        if (permissionsToRemove.Count > 0)
            _context.UserPermissions.RemoveRange(permissionsToRemove);

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

    private async Task<(bool Succeeded, string? Error, bool NotFound)> ValidateRolePermissionsAsync(
        IEnumerable<int> permissionIds)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();
        var requestedRolePermissionIds = permissionIds
            .Distinct()
            .ToArray();

        if (requestedRolePermissionIds.Length == 0)
            return (true, null, false);

        var validation = await ValidatePermissionsAsync(
            requestedRolePermissionIds,
            PermissionCategory.Crud,
            "RolesPermissions");

        if (!validation.Succeeded)
            return validation;

        var allCrudPermissionIds = await _context.Permissions
            .Where(permission => Permissions.All.Contains(permission.Key))
            .Select(permission => new
            {
                permission.Id,
                permission.Key
            })
            .ToListAsync();

        var allCrudPermissionIdSet = allCrudPermissionIds
            .Where(permission => GetPermissionCategory(permission.Key) == PermissionCategory.Crud)
            .Select(permission => permission.Id)
            .ToHashSet();

        var extraRolePermissionIds = requestedRolePermissionIds
            .Where(permissionId => !allCrudPermissionIdSet.Contains(permissionId))
            .OrderBy(permissionId => permissionId)
            .ToArray();

        if (extraRolePermissionIds.Length > 0)
        {
            return (false,
                $"RolesPermissions contém permissões que não fazem parte de todas as permissões CRUD existentes: {string.Join(", ", extraRolePermissionIds)}.",
                false);
        }

        var missingRolePermissionIds = allCrudPermissionIdSet
            .Except(requestedRolePermissionIds)
            .OrderBy(permissionId => permissionId)
            .ToArray();

        if (missingRolePermissionIds.Length > 0)
        {
            return (false,
                $"RolesPermissions deve refletir todas as permissões CRUD existentes. IDs ausentes: {string.Join(", ", missingRolePermissionIds)}.",
                false);
        }

        if (await CurrentUserHasRoleAsync(currentUser.Id, UserRole.ADMIN))
            return (true, null, false);

        return (false, "Somente Admin pode enviar RolesPermissions com todas as permissões CRUD.", false);
    }

    private async Task<bool> CurrentUserHasRoleAsync(long userId, UserRole role)
    {
        return await _context.UserRoles
            .AnyAsync(userRole =>
                userRole.UserId == userId &&
                userRole.RoleId == (long)role);
    }

    private async Task<(bool Succeeded, string? Error, bool NotFound)> ValidateExtraPermissionsAsync(
        IEnumerable<int> permissionIds)
    {
        return await ValidatePermissionsAsync(
            permissionIds.Distinct().ToArray(),
            PermissionCategory.Extra,
            "ExtrasPermissions");
    }

    private async Task<(bool Succeeded, string? Error, bool NotFound)> ValidatePermissionsAsync(
        int[] permissionIds,
        PermissionCategory expectedCategory,
        string fieldName)
    {
        if (permissionIds.Length == 0)
            return (true, null, false);

        var permissions = await _context.Permissions
            .Where(permission => permissionIds.Contains(permission.Id))
            .Select(permission => new
            {
                permission.Id,
                permission.Key
            })
            .ToListAsync();

        var invalidPermissionIds = permissionIds
            .Except(permissions.Select(permission => permission.Id))
            .OrderBy(permissionId => permissionId)
            .ToArray();

        if (invalidPermissionIds.Length > 0)
            return (false, $"Permissões inválidas em {fieldName}: {string.Join(", ", invalidPermissionIds)}.", false);

        var wrongCategoryPermissionIds = permissions
            .Where(permission => GetPermissionCategory(permission.Key) != expectedCategory)
            .Select(permission => permission.Id)
            .OrderBy(permissionId => permissionId)
            .ToArray();

        if (wrongCategoryPermissionIds.Length > 0)
        {
            var expectedDescription = expectedCategory == PermissionCategory.Crud
                ? "CRUD"
                : "extras";

            return (false,
                $"Permissões em categoria inválida em {fieldName}. Esperado: {expectedDescription}. IDs: {string.Join(", ", wrongCategoryPermissionIds)}.",
                false);
        }

        return (true, null, false);
    }

    private async Task<List<RolePermissionInfo>> GetRolePermissionsForUserAsync(long userId)
    {
        return await _context.UserRoles
            .Where(userRole => userRole.UserId == userId)
            .Join(
                _context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new { userRole.RoleId, RoleName = role.Name ?? string.Empty })
            .Join(
                _context.RolePermissions.Include(rolePermission => rolePermission.Permission),
                userRole => userRole.RoleId,
                rolePermission => rolePermission.RoleId,
                (userRole, rolePermission) => new RolePermissionInfo
                {
                    Permission = rolePermission.Permission,
                    RoleName = userRole.RoleName
                })
            .ToListAsync();
    }

    private static PermissionCategory GetPermissionCategory(string permissionKey)
    {
        return permissionKey.EndsWith(".read", StringComparison.OrdinalIgnoreCase) ||
            permissionKey.EndsWith(".create", StringComparison.OrdinalIgnoreCase) ||
            permissionKey.EndsWith(".update", StringComparison.OrdinalIgnoreCase) ||
            permissionKey.EndsWith(".delete", StringComparison.OrdinalIgnoreCase)
                ? PermissionCategory.Crud
                : PermissionCategory.Extra;
    }

    private static PermissionResponse ToPermissionResponse(Permission permission)
    {
        return new PermissionResponse
        {
            Id = permission.Id,
            Key = permission.Key,
            Description = permission.Description
        };
    }

    private enum PermissionCategory
    {
        Crud,
        Extra
    }

    private class RolePermissionInfo
    {
        public Permission Permission { get; set; } = null!;
        public string RoleName { get; set; } = string.Empty;
    }
}
