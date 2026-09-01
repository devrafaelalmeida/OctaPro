using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OctaPro.Models;
using OctaPro.Services.interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OctaPro.Services
{
    
    public class TokenService : ITokenService

    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration,
                        UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdPublic.ToString()),
                new Claim("corporation_id", user.CorporationId.ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(GetRequiredConfigurationValue("Jwt:Key", "JWT_KEY")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: GetConfigurationValue("Jwt:Issuer", "JWT_ISSUER"),
                audience: GetConfigurationValue("Jwt:Audience", "JWT_AUDIENCE"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(GetRequiredConfigurationValue("Jwt:ExpireMinutes", "JWT_EXPIRE_MINUTES"))),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string? GetConfigurationValue(params string[] keys)
        {
            return keys
                .Select(key => _configuration[key])
                .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
        }

        private string GetRequiredConfigurationValue(params string[] keys)
        {
            return GetConfigurationValue(keys)
                ?? throw new InvalidOperationException($"Configuração obrigatória ausente: {string.Join(" ou ", keys)}");
        }
    }
}
