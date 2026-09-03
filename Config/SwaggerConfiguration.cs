using Microsoft.OpenApi;

namespace OctaPro.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "OctaPro",
                Version = "v1",
                Description = "Sistema Gerenciador de Processos Juriciários",
                Contact = new OpenApiContact
                {
                    Name = "Rafael dos Santos de Almeida",
                    Email = "almeida9137@gmail.com"
                }
            });

            // Define o esquema de segurança Bearer/JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT: Bearer {seu token}"
            });

        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
                options.RoutePrefix = string.Empty; // Swagger na raiz: http://localhost:{porta}/
            });
        }

        return app;
    }
}
