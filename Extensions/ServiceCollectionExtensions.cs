using Microsoft.Extensions.DependencyInjection;
using OctaPro.Interfaces;
using OctaPro.Models;
using OctaPro.Services;
using OctaPro.Services.interfaces;

namespace OctaPro.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IJudicialProcessService, JudicialProcessService>();  
            services.AddScoped<ISettlementService, SettlementService>();  
            services.AddScoped<ILegalFeeService, LegalFeeService>();
            services.AddScoped<IInstallmentService, InstallmentService>();
            services.AddScoped<ICorporationService, CorporationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAccessControlService, AccessControlService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ITokenRevocationService, TokenRevocationService>();
            


            return services;
        }
    }
}
