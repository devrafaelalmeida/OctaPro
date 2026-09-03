using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OctaPro.Authorization;
using OctaPro.Commands;
using OctaPro.Configurations;
using OctaPro.Data;
using OctaPro.Extensions;
using OctaPro.Middlewares;
using OctaPro.Models;
using OctaPro.Repositories;
using OctaPro.Services;
using OctaPro.Services.interfaces;
using OctaPro.Tenancy;
using OctaPro.Utils;

EnvFileLoader.Load();
SQLitePCL.Batteries_V2.Init();

var builder = WebApplication.CreateBuilder(args);
var environmentName = builder.Configuration["ENVIROMENT"] ?? "DEV";
var isProduction = string.Equals(environmentName, "PRODUCTION", StringComparison.OrdinalIgnoreCase);

// ─── Services ───────────────────────────────────────────────
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var tenantContext = serviceProvider.GetRequiredService<ITenantContext>();
    var tenant = tenantContext.Current
        ?? throw new InvalidOperationException("Tenant não resolvido para esta requisição.");

    options.UseNpgsql(TenantConnectionStringBuilder.Build(tenant));
});

var tenantDbPath = builder.Configuration.GetConnectionString("TenantDb") ?? "Data Source=tenants.db";
var tenantDbKey = GetRequiredConfigurationValue(builder.Configuration, "TENANT_DB_KEY");
var tenantConnStringBuilder = new SqliteConnectionStringBuilder(tenantDbPath)
{
    Password = tenantDbKey
};

builder.Services.AddDbContext<TenantDbContext>(options =>
    options.UseSqlite(tenantConnStringBuilder.ConnectionString)
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        if (isProduction)
        {
            policy
                .WithOrigins(GetRequiredConfigurationValue(builder.Configuration, "CORS_ALLOWED_ORIGINS").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

builder.Services
    .AddIdentity<User, IdentityRole<long>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender<User>, DummyEmailSender>();
builder.Services.AddApplicationServices();

builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Permissions.All)
    {
        options.AddPolicy(permission, policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});
builder.Services.AddSwaggerConfiguration();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddScoped<ITenantRepository, EfTenantRepository>();
builder.Services.AddScoped<ITenantContext, TenantContext>();

// ─── Build ──────────────────────────────────────────────────
var app = builder.Build();

if (args.Contains("migrate-tenants"))
{
    using var scope = app.Services.CreateScope();
    await TenantMigrationRunner.RunAsync(scope.ServiceProvider);
    return;
}

if (args.Length > 0 && args[0] == "list-migrations")
{
    if (args.Length < 2)
    {
        Console.WriteLine("Uso: dotnet run -- list-migrations <domain-do-tenant>");
        return;
    }

    var domain = args[1];
    using var scope = app.Services.CreateScope();
    await TenantMigrationInspector.ListPendingAsync(scope.ServiceProvider, domain);
    return;
}

// ─── Middleware Pipeline ─────────────────────────────────────
app.UseSwaggerConfiguration();

if (isProduction)
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Seeds agora devem ser executadas manualmente por tenant, pois o AppDbContext
// depende de um tenant resolvido durante uma requisição HTTP.

app.MapControllers();
app.MapGet("/", () => "Hello World").RequireAuthorization();

app.Run();

static string GetRequiredConfigurationValue(IConfiguration configuration, string key)
{
    return configuration[key] ?? throw new InvalidOperationException($"Configuração obrigatória ausente: {key}");
}
