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
            


            return services;
        }
    }
}
