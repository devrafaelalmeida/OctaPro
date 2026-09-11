using Microsoft.AspNetCore.Identity;
using OctaPro.DTO.Response;
using OctaPro.Models;
using OctaPro.Repositories;
using OctaPro.Tenancy;

namespace OctaPro.Data.Seeds;

public static class SeedRunner
{
    public static async Task RunAsync(IServiceProvider rootServices, string? domain = null)
    {
        var tenants = await GetTenantsAsync(rootServices, domain);

        if (tenants.Count == 0)
        {
            Console.WriteLine("[SeedRunner] Nenhum tenant ativo encontrado.");
            return;
        }

        foreach (var tenant in tenants)
        {
            await RunTenantSeedsAsync(rootServices, tenant);
        }
    }

    public static async Task RunOneAsync(IServiceProvider rootServices, string seedName, string? domain = null)
    {
        var normalizedSeedName = seedName.Trim().ToLowerInvariant();
        var tenants = await GetTenantsAsync(rootServices, domain);

        if (tenants.Count == 0)
        {
            Console.WriteLine("[SeedRunner] Nenhum tenant ativo encontrado.");
            return;
        }

        foreach (var tenant in tenants)
        {
            await RunTenantSeedAsync(rootServices, tenant, normalizedSeedName);
        }
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

    private static async Task RunTenantSeedsAsync(IServiceProvider rootServices, TenantDto tenant)
    {
        Console.WriteLine($"[SeedRunner] Rodando seeds no tenant '{tenant.ConnectionName}' (domain: {tenant.Domain})...");

        using var scope = rootServices.CreateScope();

        var tenantContext = scope.ServiceProvider.GetRequiredService<ITenantContext>();
        tenantContext.SetTenant(tenant);

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

        await CorporationSeeder.SeedInitialCorporationAsync(context);
        await RoleSeeder.SeedRolesAsync(roleManager);
        await PermissionSeeder.SeedPermissionsAsync(context);
        await StatusReferenceSeeder.SeedStatusReferencesAsync(context);
        await AdminUserSeeder.RunTenantAsync(rootServices, tenant);

        Console.WriteLine($"[SeedRunner] Seeds finalizadas no tenant '{tenant.ConnectionName}'.");
        Console.WriteLine("=======================================================================================");
    }

    private static async Task RunTenantSeedAsync(IServiceProvider rootServices, TenantDto tenant, string seedName)
    {
        Console.WriteLine($"[SeedRunner] Rodando seed '{seedName}' no tenant '{tenant.ConnectionName}' (domain: {tenant.Domain})...");

        using var scope = rootServices.CreateScope();

        var tenantContext = scope.ServiceProvider.GetRequiredService<ITenantContext>();
        tenantContext.SetTenant(tenant);

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

        switch (seedName)
        {
            case "first_corporation":
                await CorporationSeeder.SeedInitialCorporationAsync(context);
                break;

            case "roles":
                await RoleSeeder.SeedRolesAsync(roleManager);
                break;

            case "permissions":
                await PermissionSeeder.SeedPermissionsAsync(context);
                break;

            case "status_reference":
            case "status_references":
                await StatusReferenceSeeder.SeedStatusReferencesAsync(context);
                break;

            case "admin_user":
                await AdminUserSeeder.RunAsync(rootServices, tenant.Domain);
                break;

            default:
                throw new ArgumentException($"Seed desconhecida: {seedName}. Use: first_corporation, roles, permissions, status_reference ou admin_user.");
        }

        Console.WriteLine($"[SeedRunner] Seed '{seedName}' finalizada no tenant '{tenant.ConnectionName}'.");
        Console.WriteLine("=======================================================================================");
    }
}
