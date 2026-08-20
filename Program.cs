using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OctaPro.Configurations;
using OctaPro.Data;
using OctaPro.Data.Seeds;
using OctaPro.Extensions;
using OctaPro.Models;
using OctaPro.Services;
using OctaPro.Services.interfaces;
using OctaPro.Utils;

EnvFileLoader.Load();

var builder = WebApplication.CreateBuilder(args);
var environmentName = builder.Configuration["ENVIROMENT"] ?? "DEV";
var isProduction = string.Equals(environmentName, "PRODUCTION", StringComparison.OrdinalIgnoreCase);

// ─── Services ───────────────────────────────────────────────
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(BuildConnectionString(builder.Configuration))
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
builder.Services.AddSwaggerConfiguration();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ─── Build ──────────────────────────────────────────────────
var app = builder.Build();

// ─── Middleware Pipeline ─────────────────────────────────────
app.UseSwaggerConfiguration();

if (isProduction)
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole<long>>>();

    await RoleSeeder.SeedRolesAsync(roleManager);
}

app.MapControllers();
app.MapGet("/", () => "Hello World").RequireAuthorization();

app.Run();

static string BuildConnectionString(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    if (!string.IsNullOrWhiteSpace(connectionString))
        return connectionString;

    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = GetRequiredConfigurationValue(configuration, "DB_HOST"),
        Port = int.Parse(GetRequiredConfigurationValue(configuration, "DB_PORT")),
        Database = GetRequiredConfigurationValue(configuration, "DB_NAME"),
        Username = GetRequiredConfigurationValue(configuration, "DB_USER"),
        Password = GetRequiredConfigurationValue(configuration, "DB_PASSWORD")
    };

    return builder.ConnectionString;
}

static string GetRequiredConfigurationValue(IConfiguration configuration, string key)
{
    return configuration[key] ?? throw new InvalidOperationException($"Configuração obrigatória ausente: {key}");
}
