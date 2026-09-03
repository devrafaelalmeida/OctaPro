using OctaPro.DTO.Response;

namespace OctaPro.Repositories;

public interface ITenantRepository
{
    Task<TenantDto?> GetByDomainAsync(string domain);

    Task<IEnumerable<TenantDto>> GetAllAsync();
}
