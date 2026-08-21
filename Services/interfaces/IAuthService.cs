using OctaPro.Models;

namespace OctaPro.Services.interfaces
{
    
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<User?> GetCurrentUserAsync();
    }
}
