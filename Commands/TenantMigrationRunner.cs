using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.Repositories;
using OctaPro.Tenancy;

namespace OctaPro.Commands;

public static class TenantMigrationRunner
{
    public static async Task RunAsync(IServiceProvider services)
    {
        var tenantRepository = services.GetRequiredService<ITenantRepository>();
        var tenants = (await tenantRepository.GetAllAsync()).ToList();
        var successCount = 0;
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

        Console.WriteLine($"[MigrationRunner] Finalizado. Sucesso: {successCount}. Falhas: {failureCount}.");
    }
}
