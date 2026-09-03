using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.Repositories;
using OctaPro.Tenancy;

namespace OctaPro.Commands;

public static class TenantMigrationInspector
{
    public static async Task ListPendingAsync(IServiceProvider services, string domain)
    {
        var tenantRepository = services.GetRequiredService<ITenantRepository>();
        var tenant = await tenantRepository.GetByDomainAsync(domain);

        if (tenant == null)
        {
            Console.WriteLine($"[MigrationInspector] Tenant com domain '{domain}' não encontrado ou inativo.");
            return;
        }

        try
        {
            Console.WriteLine($"[MigrationInspector] Verificando migrations pendentes para tenant '{tenant.ConnectionName}' (domain: {tenant.Domain})...");

            var connectionString = TenantConnectionStringBuilder.Build(tenant);
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            await using var context = new AppDbContext(options);
            var pendingMigrations = (await context.Database.GetPendingMigrationsAsync()).ToList();

            if (pendingMigrations.Count == 0)
            {
                Console.WriteLine($"[MigrationInspector] Tenant '{tenant.ConnectionName}': nenhuma migration pendente.");
                return;
            }

            Console.WriteLine($"[MigrationInspector] Tenant '{tenant.ConnectionName}': {pendingMigrations.Count} migration(ns) pendente(s):");

            foreach (var pendingMigration in pendingMigrations)
            {
                Console.WriteLine($"  - {pendingMigration}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MigrationInspector] ERRO ao verificar migrations pendentes para domain '{domain}' ({ex.GetType().Name}): {ex.Message}");
        }
    }
}
