using OctaPro.Models;

namespace OctaPro.Services.interfaces;

public interface ICurrentUserService
{
    Task<User?> GetCurrentUserAsync();
    Task<User> GetRequiredCurrentUserAsync();
}
