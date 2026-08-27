using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OctaPro.Services.interfaces;
using System.Text;

namespace OctaPro.Configurations;

public static class JwtConfiguration
{
    public static IServiceCollection AddJwtConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var environmentName = configuration["ENVIROMENT"] ?? "DEV";
            var isProduction = string.Equals(environmentName, "PRODUCTION", StringComparison.OrdinalIgnoreCase);

            options.RequireHttpsMetadata = isProduction;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = GetConfigurationValue(configuration, "Jwt:Issuer", "JWT_ISSUER"),
                ValidAudience = GetConfigurationValue(configuration, "Jwt:Audience", "JWT_AUDIENCE"),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(GetRequiredConfigurationValue(configuration, "Jwt:Key", "JWT_KEY")))
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    const string bearerPrefix = "Bearer ";
                    var authorization = context.HttpContext.Request.Headers.Authorization.ToString();

                    if (!authorization.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                        return;

                    var token = authorization[bearerPrefix.Length..].Trim();
                    var tokenRevocationService = context.HttpContext.RequestServices
                        .GetRequiredService<ITokenRevocationService>();

                    if (await tokenRevocationService.IsTokenRevokedAsync(token))
                        context.Fail("Token revogado.");
                }
            };
        });

        return services;
    }

    private static string? GetConfigurationValue(IConfiguration configuration, params string[] keys)
    {
        return keys
            .Select(key => configuration[key])
            .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
    }

    private static string GetRequiredConfigurationValue(IConfiguration configuration, params string[] keys)
    {
        return GetConfigurationValue(configuration, keys)
            ?? throw new InvalidOperationException($"Configuração obrigatória ausente: {string.Join(" ou ", keys)}");
    }
}
