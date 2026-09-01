using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class TokenRevocationService : ITokenRevocationService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenRevocationService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RevokeCurrentTokenAsync()
    {
        var token = GetCurrentBearerToken();

        if (string.IsNullOrWhiteSpace(token))
            return;

        var expiresAt = GetTokenExpiration(token);

        if (expiresAt <= DateTime.UtcNow)
            return;

        var tokenHash = ComputeTokenHash(token);
        var alreadyRevoked = await _context.RevokedTokens
            .AnyAsync(revokedToken => revokedToken.TokenHash == tokenHash);

        if (alreadyRevoked)
            return;

        _context.RevokedTokens.Add(new RevokedToken
        {
            TokenHash = tokenHash,
            ExpiresAt = expiresAt
        });

        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsTokenRevokedAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        var tokenHash = ComputeTokenHash(token);
        var now = DateTime.UtcNow;

        return await _context.RevokedTokens
            .AnyAsync(revokedToken =>
                revokedToken.TokenHash == tokenHash &&
                revokedToken.ExpiresAt > now);
    }

    private string? GetCurrentBearerToken()
    {
        const string bearerPrefix = "Bearer ";
        var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authorization))
            return null;

        if (!authorization.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
            return null;

        return authorization[bearerPrefix.Length..].Trim();
    }

    private static DateTime GetTokenExpiration(string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwtToken.ValidTo;
    }

    private static string ComputeTokenHash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
