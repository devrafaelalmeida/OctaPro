using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Response;
using OctaPro.Repositories;
using OctaPro.Tenancy;

namespace OctaPro.Commands;

public static class TenantMigrationRunner
{
    public static async Task RunAsync(IServiceProvider services, string? domain = null)
    {
        var tenantRepository = services.GetRequiredService<ITenantRepository>();
        var tenants = await GetTenantsAsync(tenantRepository, domain);

        if (tenants.Count == 0)
        {
            Console.WriteLine($"[MigrationRunner] Tenant com domain '{domain}' não encontrado ou inativo.");
            return;
        }

        var successCount = 0;
        var skippedCount = 0;
        var failureCount = 0;

        Console.WriteLine($"[MigrationRunner] {tenants.Count} tenant(s) encontrado(s).");

        foreach (var tenant in tenants)
        {
            Console.WriteLine($"[MigrationRunner] Iniciando migration para tenant '{tenant.ConnectionName}' (domain: {tenant.Domain})...");

            try
            {
                var connectionString = TenantConnectionStringBuilder.Build(tenant);
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(connectionString)
                    .Options;

                await using var context = new AppDbContext(options);
                var pendingMigrations = (await context.Database.GetPendingMigrationsAsync()).ToList();

                if (pendingMigrations.Count == 0)
                {
                    skippedCount++;
                    Console.WriteLine($"[MigrationRunner] Tenant '{tenant.ConnectionName}': nenhuma migration pendente.");
                    Console.WriteLine("=======================================================================================");
                    continue;
                }

                Console.WriteLine($"[MigrationRunner] Tenant '{tenant.ConnectionName}': {pendingMigrations.Count} migration(ns) pendente(s).");
                await context.Database.MigrateAsync();

                successCount++;
                Console.WriteLine($"[MigrationRunner] Tenant '{tenant.ConnectionName}': migrations aplicadas com sucesso.");
                Console.WriteLine("=======================================================================================");

            }
            catch (Exception ex)
            {
                failureCount++;
                Console.WriteLine($"[MigrationRunner] ERRO ao aplicar migration no tenant '{tenant.ConnectionName}' ({ex.GetType().Name}): {ex.Message}");
                Console.WriteLine("=======================================================================================");

            }
        }

        Console.WriteLine($"[MigrationRunner] Finalizado. Aplicados: {successCount}. Sem pendências: {skippedCount}. Falhas: {failureCount}.");
    }

    private static async Task<List<TenantDto>> GetTenantsAsync(ITenantRepository tenantRepository, string? domain)
    {
        if (string.IsNullOrWhiteSpace(domain))
            return (await tenantRepository.GetAllAsync()).ToList();

        var tenant = await tenantRepository.GetByDomainAsync(domain);
        return tenant == null ? [] : [tenant];
    }
}
