using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OctaPro.Configurations;
using OctaPro.Data;
using OctaPro.Data.Seeds;
using OctaPro.Extensions;
using OctaPro.Models;
using OctaPro.Services;
using OctaPro.Services.interfaces;


var builder = WebApplication.CreateBuilder(args);

// ─── Services ───────────────────────────────────────────────
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            // .WithOrigins("http://localhost:5173", "http://localhost:5174" )
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddIdentity<User, IdentityRole<long>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
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

// app.UseHttpsRedirection();
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
app.MapIdentityApi<User>();
app.MapGet("/", () => "Hello World").RequireAuthorization();

app.Run();