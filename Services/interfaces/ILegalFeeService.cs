using OctaPro.DTO.Request;
using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces
{
    public interface ILegalFeeService
    {
        Task<IEnumerable<LegalFeeResponse>> GetAllAsync(SettlementFilterRequest filter = null!);
        Task<LegalFeeResponse?> GetByIdAsync(Guid idPublic);
        Task CreateAsync(LegalFeeRequest request, Guid userLoggedUUID);
        Task<InstallmentResponse> AddInstallmentAsync(Guid legalFeeId, InstallmentRequest request);
        Task<bool> DeleteAsync(Guid idPublic);
        Task<bool> UpdateAsync(Guid legalFeeId, LegalFeeRequest request);
    }
}
