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
    [Authorize(Policy = Permissions.EntityRead)]
    [Route("api/entity")]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _service;

        public EntityController(IEntityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntityResponse>>> GetEntities([FromQuery] EntityFilterRequest filter)
        {
            return Ok(await _service.GetEntitiesAsync(filter));
        }

        [HttpGet("{idPublic:guid}")]
        public async Task<ActionResult<EntityResponse>> GetById(Guid idPublic)
        {
            var entity = await _service.GetByIdAsync(idPublic);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost("pf")]
        [Authorize(Policy = Permissions.EntityCreate)]
        public async Task<IActionResult> CreatePF(EntityIndividualRequest request)
        {
            await _service.CreateEntityIndividualAsync(request);
            return StatusCode(201);
        }

        [HttpPost("pj")]
        [Authorize(Policy = Permissions.EntityCreate)]
        public async Task<IActionResult> CreatePJ(EntityCompanyRequest request)
        {
            await _service.CreateEntityCompanyAsync(request);
            return StatusCode(201);
        }

        [HttpPut("pf/{entityId:guid}")]
        [Authorize(Policy = Permissions.EntityUpdate)]
        public async Task<IActionResult> UpdatePF(Guid entityId, EntityIndividualRequest request)
        {
            if (!await _service.UpdateEntityIndividualAsync(entityId, request))
                return NotFound();

            return NoContent();
        }

        [HttpPut("pj/{entityId:guid}")]
        [Authorize(Policy = Permissions.EntityUpdate)]
        public async Task<IActionResult> UpdatePJ(Guid entityId, EntityCompanyRequest request)
        {
            if (!await _service.UpdateEntityCompanyAsync(entityId, request))
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{entityId:guid}")]
        [Authorize(Policy = Permissions.EntityDelete)]
        public async Task<IActionResult> Delete(Guid entityId)
        {
            if (!await _service.DeleteEntityAsync(entityId))
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EntitySelectResponse>>>SearchClients([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Ok(new List<EntitySelectResponse>());

            searchTerm = searchTerm.ToLower();

            var result = await _service.SearchClientsAsync(searchTerm);

            return Ok(result);
        }






    }
}
