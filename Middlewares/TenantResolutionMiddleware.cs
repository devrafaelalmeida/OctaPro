using OctaPro.Repositories;
using OctaPro.Tenancy;

namespace OctaPro.Middlewares;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenantRepository tenantRepository,
        ITenantContext tenantContext)
    {
        var host = context.Request.Host.Host;
        var tenant = await tenantRepository.GetByDomainAsync(host);

        if (tenant == null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Tenant não encontrado.");
            return;
        }

        tenantContext.SetTenant(tenant);

        await _next(context);
    }
}
