using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.Authorization;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers
{
    [ApiController]
    [Route("api/installments")]
    public class InstallmentController : ControllerBase
    {
        private readonly IInstallmentService _installmentService;

        public InstallmentController(IInstallmentService installmentService)
        {
            _installmentService = installmentService;
        }

        [HttpPut("reverse-installments")]
        [Authorize(Policy = Permissions.InstallmentReverse)]
        public async Task<ActionResult<ReverseInstallmentResult>> ReverseInstallments(ReverseInstallmentRequest request)
        {
            var result = await _installmentService.ReverseInstallmentsAsync(request.TypeId, request.ReferenceId, request.Ids);

            return Ok(result);
        }
    }
}
