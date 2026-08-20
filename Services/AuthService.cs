using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    CreatedAt = DateTime.UtcNow,
                    IdPublic = Guid.NewGuid()
                };
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                    return result;

                await _userManager.AddToRoleAsync(user, "Common");

                return result;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (!result.Succeeded)
                return null;

            return await _tokenService.GenerateTokenAsync(user);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;

            var idPublicClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (idPublicClaim == null)
                return null;

            if (!Guid.TryParse(idPublicClaim, out var idPublic))
                return null;

            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.IdPublic == idPublic);
        }
    }
    
}
