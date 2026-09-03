using Npgsql;
using OctaPro.DTO.Response;

namespace OctaPro.Tenancy;

public static class TenantConnectionStringBuilder
{
    public static string Build(TenantDto tenant)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = tenant.DataSource,
            Database = tenant.Database,
            Username = tenant.Username,
            Password = tenant.Password
        };

        return builder.ConnectionString;
    }
}
