using OctaPro.DTO;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;

namespace OctaPro.Services.interfaces
{
    public interface IJudicialProcessService
    {
        Task<IEnumerable<JudicialProcessResponse>> GetAllAsync(ProcessFilterRequest filter = null!);
        Task<JudicialProcessResponse?> GetByIdAsync(Guid idPublic);
        Task CreateAsync(JudicialProcessRequest request, Guid userLoggedUUID);
        Task<bool> ArchiveAsync(Guid idPublic);
        Task<bool> DeleteAsync(Guid idPublic);
        Task<IEnumerable<SelectOptionResponse>> GetAllNatureAsync();
        Task<IEnumerable<SelectOptionResponse>> GetActionsAsync(int natureId);

        Task<IEnumerable<SelectOptionResponse>> searchProcessAsync(string searchTerm);




    }
}
