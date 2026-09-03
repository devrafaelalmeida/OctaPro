using OctaPro.DTO.Response;

namespace OctaPro.Tenancy;

public interface ITenantContext
{
    TenantDto? Current { get; }

    void SetTenant(TenantDto tenant);
}
