using Microsoft.EntityFrameworkCore;
using OctaPro.Authorization;
using OctaPro.Enums;
using OctaPro.Models;

namespace OctaPro.Data.Seeds;

public static class PermissionSeeder
{
    private static readonly IReadOnlyDictionary<string, string> PermissionDescriptions = new Dictionary<string, string>
    {
        [Permissions.CorporationRead] = "Visualizar empresas",
        [Permissions.CorporationCreate] = "Criar empresas",
        [Permissions.CorporationUpdate] = "Editar empresas",
        [Permissions.CorporationDelete] = "Excluir empresas",

        [Permissions.EntityRead] = "Visualizar entidades",
        [Permissions.EntityCreate] = "Criar entidades",
        [Permissions.EntityUpdate] = "Editar entidades",
        [Permissions.EntityDelete] = "Excluir entidades",

        [Permissions.InstallmentReverse] = "Reverter parcelas",

        [Permissions.JudicialProcessRead] = "Visualizar processos judiciais",
        [Permissions.JudicialProcessCreate] = "Criar processos judiciais",
        [Permissions.JudicialProcessUpdate] = "Editar processos judiciais",
        [Permissions.JudicialProcessArchive] = "Arquivar processos judiciais",
        [Permissions.JudicialProcessDelete] = "Excluir processos judiciais",

        [Permissions.LegalFeeRead] = "Visualizar honorários",
        [Permissions.LegalFeeCreate] = "Criar honorários",
        [Permissions.LegalFeeUpdate] = "Editar honorários",
        [Permissions.LegalFeeDelete] = "Excluir honorários",
        [Permissions.LegalFeeAddInstallment] = "Adicionar parcela em honorários",

        [Permissions.SettlementRead] = "Visualizar acordos",
        [Permissions.SettlementCreate] = "Criar acordos",
        [Permissions.SettlementUpdate] = "Editar acordos",
        [Permissions.SettlementDelete] = "Excluir acordos",
        [Permissions.SettlementAddInstallment] = "Adicionar parcela em acordos",

        [Permissions.UserRead] = "Visualizar usuários",
        [Permissions.UserCreate] = "Criar usuários",
        [Permissions.UserUpdate] = "Editar usuários",
        [Permissions.UserDelete] = "Excluir usuários"
    };

    private static readonly IReadOnlyDictionary<UserRole, string[]> RolePermissions = new Dictionary<UserRole, string[]>
    {
        [UserRole.ADMIN] = PermissionDescriptions.Keys.ToArray(),
        [UserRole.MANAGER] = PermissionDescriptions.Keys.ToArray(),
        [UserRole.COMMON] =
        [
            Permissions.CorporationRead,
            Permissions.EntityRead,
            Permissions.JudicialProcessRead,
            Permissions.LegalFeeRead,
            Permissions.SettlementRead,
            Permissions.UserRead
        ]
    };

    public static async Task SeedPermissionsAsync(AppDbContext context)
    {
        await UpsertPermissionsAsync(context);
        await UpsertRolePermissionsAsync(context);
    }

    //Update/Insert Permissions
    private static async Task UpsertPermissionsAsync(AppDbContext context)
    {
        var existingPermissions = await context.Permissions.ToDictionaryAsync(permission => permission.Key);

        foreach (var (key, description) in PermissionDescriptions)
        {
            if (existingPermissions.TryGetValue(key, out var permission))
            {
                permission.Description = description;
                permission.UpdatedAt = DateTime.UtcNow;
                continue;
            }

            context.Permissions.Add(new Permission
            {
                Key = key,
                Description = description
            });
        }

        await context.SaveChangesAsync();
    }

    // Update/Insert link between role(s) and permission(s)
    private static async Task UpsertRolePermissionsAsync(AppDbContext context)
    {
        var permissionIdsByKey = await context.Permissions
            .ToDictionaryAsync(permission => permission.Key, permission => permission.Id);

        var existingRolePermissions = await context.RolePermissions
            .Select(rolePermission => new
            {
                rolePermission.RoleId,
                rolePermission.PermissionId
            })
            .ToListAsync();

        var existingKeys = existingRolePermissions
            .Select(rolePermission => (rolePermission.RoleId, rolePermission.PermissionId))
            .ToHashSet();

        foreach (var (role, permissions) in RolePermissions)
        {
            var roleId = (long)role;

            foreach (var permissionKey in permissions)
            {
                var permissionId = permissionIdsByKey[permissionKey];

                if (existingKeys.Contains((roleId, permissionId)))
                    continue;

                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
