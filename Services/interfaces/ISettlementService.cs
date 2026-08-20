using OctaPro.DTO;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces
{
    public interface ISettlementService
    {
        Task<IEnumerable<SettlementResponse>> GetAllAsync(SettlementFilterRequest filter = null!);
        Task<SettlementResponse?> GetByIdAsync(Guid idPublic);
        Task CreateAsync(SettlementRequest request, Guid userLoggedUUID);
        Task<InstallmentResponse> AddInstallmentAsync(Guid settlementId, InstallmentRequest request);
        Task<bool> DeleteAsync(Guid idPublic);
        Task<bool> UpdateAsync(Guid settlementId, SettlementRequest request);



    }
}
