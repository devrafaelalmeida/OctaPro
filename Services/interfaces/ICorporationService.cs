using OctaPro.DTO.Request;
using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces;

public interface ICorporationService
{
    Task<IEnumerable<CorporationResponse>> GetAllAsync();
    Task<CorporationResponse?> GetByIdAsync(Guid idPublic);
    Task<CorporationResponse> CreateAsync(CorporationRequest request);
    Task<CorporationResponse?> UpdateAsync(Guid idPublic, CorporationRequest request);
    Task<bool> DeleteAsync(Guid idPublic);
}
