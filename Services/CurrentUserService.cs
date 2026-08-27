using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var idPublicClaim = _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(idPublicClaim, out var idPublic))
            return null;

        return await _context.Users
            .FirstOrDefaultAsync(user => user.IdPublic == idPublic);
    }

    public async Task<User> GetRequiredCurrentUserAsync()
    {
        return await GetCurrentUserAsync()
            ?? throw new UnauthorizedAccessException("Usuário autenticado não encontrado.");
    }
}
