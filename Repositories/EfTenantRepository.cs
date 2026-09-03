using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Response;

namespace OctaPro.Repositories;

public class EfTenantRepository : ITenantRepository
{
    private readonly TenantDbContext _context;

    public EfTenantRepository(TenantDbContext context)
    {
        _context = context;
    }

    public Task<TenantDto?> GetByDomainAsync(string domain)
    {
        return _context.Tenants
            .AsNoTracking()
            .Where(tenant => tenant.Domain == domain && tenant.Ativo)
            .Select(tenant => MapToDto(tenant))
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TenantDto>> GetAllAsync()
    {
        return await _context.Tenants
            .AsNoTracking()
            .Where(tenant => tenant.Ativo)
            .Select(tenant => MapToDto(tenant))
            .ToListAsync();
    }

    private static TenantDto MapToDto(Models.Tenant tenant)
    {
        return new TenantDto
        {
            Domain = tenant.Domain,
            ConnectionName = tenant.ConnectionName,
            DataSource = tenant.DataSource,
            Database = tenant.Database,
            Username = tenant.Username,
            Password = tenant.Password
        };
    }
}
