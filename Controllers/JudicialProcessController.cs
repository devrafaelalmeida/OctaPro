using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.Authorization;
using OctaPro.DTO;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers
{
    [ApiController]
    [Authorize(Policy = Permissions.JudicialProcessRead)]
    [Route("api/process")]
    public class JudicialProcessController : ControllerBase
    {
        private readonly IJudicialProcessService _service;

        public JudicialProcessController(IJudicialProcessService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JudicialProcessResponse>>> GetAll([FromQuery] ProcessFilterRequest filter)
        {
            return Ok(await _service.GetAllAsync(filter));
        }

        [HttpGet("{idPublic:guid}")]
        public async Task<ActionResult<JudicialProcessResponse>> GetById(Guid idPublic)
        {
            var process = await _service.GetByIdAsync(idPublic);
            if (process == null)
                return NotFound();

            return Ok(process);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.JudicialProcessCreate)]
        public async Task<IActionResult> SaveProcess(JudicialProcessRequest request)
        {
            string? userLoggedUUID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userLoggedUUID == null)
            {
                return Unauthorized();
            }

            await _service.CreateAsync(request, Guid.Parse(userLoggedUUID));
            return StatusCode(201);
        }

        [HttpPatch("{idPublic:guid}/archive")]
        [Authorize(Policy = Permissions.JudicialProcessArchive)]
        public async Task<IActionResult> ArchiveProcess(Guid idPublic)
        {
            if (!await _service.ArchiveAsync(idPublic))
                return NotFound("Processo judicial não encontrado.");

            return NoContent();
        }

        [HttpDelete("{idPublic:guid}")]
        [Authorize(Policy = Permissions.JudicialProcessDelete)]
        public async Task<IActionResult> DeleteEntity(Guid idPublic)
        {
            if (!await _service.DeleteAsync(idPublic))
                return NotFound("Entidade não encontrada.");

            return NoContent();
        }

        [HttpGet("natures")]
        public async Task<ActionResult<IEnumerable<SelectOptionResponse>>> GetAllNatures()
        {
            return Ok(await _service.GetAllNatureAsync());
        }

        [HttpGet("actions/{natureId:int}")]
        public async Task<ActionResult<IEnumerable<SelectOptionResponse>>> GetAllActions(int natureId)
        {
            return Ok(await _service.GetActionsAsync(natureId));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SelectOptionResponse>>> SearchProcess([FromQuery] string searchTerm)
        {
            var processes = await _service.searchProcessAsync(searchTerm);
            return Ok(processes);
        }
    }
}
