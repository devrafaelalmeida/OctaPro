using Microsoft.AspNetCore.Identity;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<UserResponse?> GetByIdAsync(Guid idPublic);
    Task<(IdentityResult Result, UserResponse? User)> CreateAsync(UserRequest request);
    Task<(IdentityResult Result, UserResponse? User)> UpdateAsync(Guid idPublic, UserRequest request);
    Task<bool> DeleteAsync(Guid idPublic);
}
