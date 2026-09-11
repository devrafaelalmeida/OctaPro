using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using OctaPro.Utils;

namespace OctaPro.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        EnvFileLoader.Load();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(BuildConnectionString(configuration))
            .Options;

        return new AppDbContext(options);
    }

    private static string BuildConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrWhiteSpace(connectionString))
            return connectionString;

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = GetRequiredConfigurationValue(configuration, "DB_HOST"),
            Port = int.Parse(GetRequiredConfigurationValue(configuration, "DB_PORT")),
            Database = GetRequiredConfigurationValue(configuration, "DB_NAME"),
            Username = GetRequiredConfigurationValue(configuration, "DB_USER"),
            Password = GetRequiredConfigurationValue(configuration, "DB_PASSWORD")
        };

        return builder.ConnectionString;
    }

    private static string GetRequiredConfigurationValue(IConfiguration configuration, string key)
    {
        return configuration[key] ?? throw new InvalidOperationException($"Configuracao obrigatoria ausente: {key}");
    }
}
