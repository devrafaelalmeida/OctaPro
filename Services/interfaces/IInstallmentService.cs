using OctaPro.DTO.Request;
using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces
{
    public interface IInstallmentService
    {

        Task<ReverseInstallmentResult> ReverseInstallmentsAsync(int typeId, Guid ReferencePublicId, List<Guid> installmentPublicIds);
    }
}
