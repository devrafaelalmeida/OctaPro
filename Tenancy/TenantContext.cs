using OctaPro.DTO.Response;

namespace OctaPro.Tenancy;

public class TenantContext : ITenantContext
{
    public TenantDto? Current { get; private set; }

    public void SetTenant(TenantDto tenant)
    {
        Current = tenant;
    }
}
