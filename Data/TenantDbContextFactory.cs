using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OctaPro.Utils;

namespace OctaPro.Data;

public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        EnvFileLoader.Load();
        SQLitePCL.Batteries_V2.Init();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var tenantDbPath = configuration.GetConnectionString("TenantDb") ?? "Data Source=tenants.db";
        var tenantDbKey = configuration["TENANT_DB_KEY"]
            ?? throw new InvalidOperationException("Configuracao obrigatoria ausente: TENANT_DB_KEY");

        var connectionStringBuilder = new SqliteConnectionStringBuilder(tenantDbPath)
        {
            Password = tenantDbKey
        };

        var options = new DbContextOptionsBuilder<TenantDbContext>()
            .UseSqlite(connectionStringBuilder.ConnectionString)
            .Options;

        return new TenantDbContext(options);
    }
}
