using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Enums;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        UserManager<User> userManager,
        AppDbContext context,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        return await _userManager.Users
            .Where(u => u.CorporationId == currentUser.CorporationId)
            .OrderBy(u => u.UserName)
            .Select(u => ToResponse(
                u,
                _context.UserRoles
                    .Where(userRole => userRole.UserId == u.Id)
                    .Select(userRole => (long?)userRole.RoleId)
                    .FirstOrDefault()))
            .ToListAsync();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid idPublic)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        return await _userManager.Users
            .Where(u =>
                u.IdPublic == idPublic &&
                u.CorporationId == currentUser.CorporationId)
            .Select(u => ToResponse(
                u,
                _context.UserRoles
                    .Where(userRole => userRole.UserId == u.Id)
                    .Select(userRole => (long?)userRole.RoleId)
                    .FirstOrDefault()))
            .FirstOrDefaultAsync();
    }

    public async Task<(IdentityResult Result, UserResponse? User)> CreateAsync(UserRequest request)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        if (request.CorporationId != currentUser.CorporationId)
            return (CorporationMismatchResult(), null);

        var roleValidation = await ValidateRoleAsync(request.RoleId);
        if (!roleValidation.Succeeded)
            return (roleValidation, null);

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return (IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordRequired",
                Description = "A senha é obrigatória."
            }), null);
        }

        var user = new User();
        ApplyRequest(user, request);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        user.IdPublic = Guid.NewGuid();

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return (result, null);

        _context.UserRoles.Add(new IdentityUserRole<long>
        {
            UserId = user.Id,
            RoleId = request.RoleId
        });
        await _context.SaveChangesAsync();

        return (result, ToResponse(user, request.RoleId));
    }

    public async Task<(IdentityResult Result, UserResponse? User)> UpdateAsync(Guid idPublic, UserRequest request)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        if (request.CorporationId != currentUser.CorporationId)
            return (CorporationMismatchResult(), null);

        var roleValidation = await ValidateRoleAsync(request.RoleId);
        if (!roleValidation.Succeeded)
            return (roleValidation, null);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.IdPublic == idPublic &&
                u.CorporationId == currentUser.CorporationId);

        if (user == null)
            return (IdentityResult.Success, null);

        ApplyRequest(user, request);
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return (result, null);

        await SyncUserRoleAsync(user.Id, request.RoleId);

        return (result, ToResponse(user, request.RoleId));
    }

    public async Task<bool> DeleteAsync(Guid idPublic)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.IdPublic == idPublic &&
                u.CorporationId == currentUser.CorporationId);

        if (user == null)
            return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    private static IdentityResult CorporationMismatchResult()
    {
        return IdentityResult.Failed(new IdentityError
        {
            Code = "CorporationMismatch",
            Description = "Usuário não pode gerenciar registros de outra empresa."
        });
    }

    private static void ApplyRequest(User user, UserRequest request)
    {
        user.UserName = request.UserName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.CorporationId = request.CorporationId;
        user.CPF = request.CPF;
        user.BirthDate = request.BirthDate;
        user.CEP = request.CEP;
        user.UF = request.UF.Trim().ToUpper();
        user.City = request.City;
        user.Address = request.Address;
        user.NumberHouse = request.NumberHouse;
        user.Complement = request.Complement;
        user.Neithborhood = request.Neithborhood;
    }

    private async Task<IdentityResult> ValidateRoleAsync(int roleId)
    {
        if (!Enum.IsDefined(typeof(UserRole), roleId))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidRoleId",
                Description = "RoleId inválido."
            });
        }

        var roleExists = await _context.Roles.AnyAsync(role => role.Id == roleId);

        if (!roleExists)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "RoleNotFound",
                Description = "Role não encontrada."
            });
        }

        return IdentityResult.Success;
    }

    private async Task SyncUserRoleAsync(long userId, int roleId)
    {
        var currentRoles = await _context.UserRoles
            .Where(userRole => userRole.UserId == userId)
            .ToListAsync();

        if (currentRoles.Count > 0)
            _context.UserRoles.RemoveRange(currentRoles);

        _context.UserRoles.Add(new IdentityUserRole<long>
        {
            UserId = userId,
            RoleId = roleId
        });

        await _context.SaveChangesAsync();
    }

    private static UserResponse ToResponse(User user, long? roleId)
    {
        return new UserResponse
        {
            IdPublic = user.IdPublic,
            UserName = user.UserName,
            Email = user.Email,
            CPF = user.CPF,
            BirthDate = user.BirthDate,
            PhoneNumber = user.PhoneNumber,
            CorporationId = user.CorporationId,
            RoleId = roleId,
            CEP = user.CEP,
            UF = user.UF,
            City = user.City,
            Address = user.Address,
            NumberHouse = user.NumberHouse,
            Complement = user.Complement,
            Neithborhood = user.Neithborhood,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
