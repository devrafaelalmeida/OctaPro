using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OctaPro.DTO.Response;
using OctaPro.Models;
using OctaPro.Repositories;
using OctaPro.Tenancy;

namespace OctaPro.Data.Seeds;

public static class AdminUserSeeder
{
    private const long InitialCorporationId = 1;

    public static async Task RunAsync(IServiceProvider rootServices, string? domain = null)
    {
        var tenants = await GetTenantsAsync(rootServices, domain);

        if (tenants.Count == 0)
        {
            Console.WriteLine("[AdminUserSeeder] Nenhum tenant ativo encontrado.");
            return;
        }

        foreach (var tenant in tenants)
        {
            await RunTenantAsync(rootServices, tenant);
        }
    }

    public static async Task RunTenantAsync(IServiceProvider rootServices, TenantDto tenant)
    {
        await SeedTenantAsync(rootServices, tenant);
    }

    private static async Task<List<TenantDto>> GetTenantsAsync(IServiceProvider rootServices, string? domain)
    {
        using var scope = rootServices.CreateScope();
        var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();

        if (!string.IsNullOrWhiteSpace(domain))
        {
            var tenant = await tenantRepository.GetByDomainAsync(domain);
            return tenant is null ? [] : [tenant];
        }

        return (await tenantRepository.GetAllAsync()).ToList();
    }

    private static async Task SeedTenantAsync(IServiceProvider rootServices, TenantDto tenant)
    {
        Console.WriteLine($"[AdminUserSeeder] Seed admin no tenant '{tenant.ConnectionName}' (domain: {tenant.Domain})...");

        using var scope = rootServices.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var adminEmail = GetRequiredConfigurationValue(configuration, "ADMIN_EMAIL");
        var adminPassword = GetRequiredConfigurationValue(configuration, "ADMIN_PASSWORD");
        var adminRole = GetRequiredConfigurationValue(configuration, "ADMIN_ROLE");

        var tenantContext = scope.ServiceProvider.GetRequiredService<ITenantContext>();
        tenantContext.SetTenant(tenant);

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        await CorporationSeeder.SeedInitialCorporationAsync(context);
        await RoleSeeder.SeedRolesAsync(roleManager);

        var user = await userManager.Users.FirstOrDefaultAsync(currentUser => currentUser.Email == adminEmail);

        if (user is null)
        {
            user = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                CorporationId = InitialCorporationId
            };

            var createResult = await userManager.CreateAsync(user, adminPassword);
            ThrowIfFailed(createResult, "criar usuario admin");
        }
        else
        {
            user.UserName = adminEmail;
            user.Email = adminEmail;
            user.EmailConfirmed = true;
            user.CorporationId = InitialCorporationId;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await userManager.UpdateAsync(user);
            ThrowIfFailed(updateResult, "atualizar usuario admin");

            if (await userManager.HasPasswordAsync(user))
            {
                var removePasswordResult = await userManager.RemovePasswordAsync(user);
                ThrowIfFailed(removePasswordResult, "remover senha anterior do admin");
            }

            var addPasswordResult = await userManager.AddPasswordAsync(user, adminPassword);
            ThrowIfFailed(addPasswordResult, "definir senha do admin");
        }

        if (!await userManager.IsInRoleAsync(user, adminRole))
        {
            var addRoleResult = await userManager.AddToRoleAsync(user, adminRole);
            ThrowIfFailed(addRoleResult, "vincular admin a role Admin");
        }

        Console.WriteLine($"[AdminUserSeeder] Admin pronto: {adminEmail}");
    }

    private static string GetRequiredConfigurationValue(IConfiguration configuration, string key)
    {
        return configuration[key] ?? throw new InvalidOperationException($"Configuracao obrigatoria ausente: {key}");
    }

    private static void ThrowIfFailed(IdentityResult result, string action)
    {
        if (result.Succeeded)
            return;

        var errors = string.Join("; ", result.Errors.Select(error => $"{error.Code}: {error.Description}"));
        throw new InvalidOperationException($"Falha ao {action}: {errors}");
    }
}
