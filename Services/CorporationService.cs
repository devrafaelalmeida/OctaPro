using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Models;
using OctaPro.Services.interfaces;

namespace OctaPro.Services;

public class CorporationService : ICorporationService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CorporationService(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CorporationResponse>> GetAllAsync()
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        return await _context.Corporations
            .Where(c => c.Id == currentUser.CorporationId)
            .OrderBy(c => c.TradeName)
            .Select(c => ToResponse(c))
            .ToListAsync();
    }

    public async Task<CorporationResponse?> GetByIdAsync(Guid idPublic)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        return await _context.Corporations
            .Where(c =>
                c.IdPublic == idPublic &&
                c.Id == currentUser.CorporationId)
            .Select(c => ToResponse(c))
            .FirstOrDefaultAsync();
    }

    public async Task<CorporationResponse> CreateAsync(CorporationRequest request)
    {
        var corporation = new Corporation();
        ApplyRequest(corporation, request);

        _context.Corporations.Add(corporation);
        await _context.SaveChangesAsync();

        return ToResponse(corporation);
    }

    public async Task<CorporationResponse?> UpdateAsync(Guid idPublic, CorporationRequest request)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        var corporation = await _context.Corporations
            .FirstOrDefaultAsync(c =>
                c.IdPublic == idPublic &&
                c.Id == currentUser.CorporationId);

        if (corporation == null)
            return null;

        ApplyRequest(corporation, request);
        corporation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ToResponse(corporation);
    }

    public async Task<bool> DeleteAsync(Guid idPublic)
    {
        var currentUser = await _currentUserService.GetRequiredCurrentUserAsync();

        var corporation = await _context.Corporations
            .FirstOrDefaultAsync(c =>
                c.IdPublic == idPublic &&
                c.Id == currentUser.CorporationId);

        if (corporation == null)
            return false;

        _context.Corporations.Remove(corporation);
        await _context.SaveChangesAsync();

        return true;
    }

    private static void ApplyRequest(Corporation corporation, CorporationRequest request)
    {
        corporation.LegalName = request.LegalName;
        corporation.TradeName = request.TradeName;
        corporation.Cnpj = request.Cnpj;
        corporation.OpeningDate = request.OpeningDate;
        corporation.StateRegistration = request.StateRegistration;
        corporation.MunicipalRegistration = request.MunicipalRegistration;
        corporation.TaxRegime = request.TaxRegime;
        corporation.ZipCode = request.ZipCode;
        corporation.Street = request.Street;
        corporation.Number = request.Number;
        corporation.Complement = request.Complement;
        corporation.District = request.District;
        corporation.City = request.City;
        corporation.State = request.State.Trim().ToUpper();
        corporation.Email = request.Email;
        corporation.Mobile = request.Mobile;
        corporation.Phone = request.Phone;
        corporation.IsActive = request.IsActive;
    }

    private static CorporationResponse ToResponse(Corporation corporation)
    {
        return new CorporationResponse
        {
            IdPublic = corporation.IdPublic,
            LegalName = corporation.LegalName,
            TradeName = corporation.TradeName,
            Cnpj = corporation.Cnpj,
            OpeningDate = corporation.OpeningDate,
            StateRegistration = corporation.StateRegistration,
            MunicipalRegistration = corporation.MunicipalRegistration,
            TaxRegime = corporation.TaxRegime,
            ZipCode = corporation.ZipCode,
            Street = corporation.Street,
            Number = corporation.Number,
            Complement = corporation.Complement,
            District = corporation.District,
            City = corporation.City,
            State = corporation.State,
            Email = corporation.Email,
            Mobile = corporation.Mobile,
            Phone = corporation.Phone,
            IsActive = corporation.IsActive,
            CreatedAt = corporation.CreatedAt,
            UpdatedAt = corporation.UpdatedAt
        };
    }
}
