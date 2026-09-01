using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OctaPro.Authorization;
using OctaPro.DTO.Request;
using OctaPro.DTO.Response;
using OctaPro.Services.interfaces;

namespace OctaPro.Controllers;

[ApiController]
[Authorize(Policy = Permissions.UserRead)]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{idPublic:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(Guid idPublic)
    {
        var user = await _service.GetByIdAsync(idPublic);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost("register")]
    [Authorize(Policy = Permissions.UserCreate)]
    public async Task<ActionResult<UserResponse>> Create(UserRequest request)
    {
        var (result, user) = await _service.CreateAsync(request);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetById), new { idPublic = user!.IdPublic }, user);
    }

    [HttpPut("{idPublic:guid}")]
    [Authorize(Policy = Permissions.UserUpdate)]
    public async Task<ActionResult<UserResponse>> Update(Guid idPublic, UserRequest request)
    {
        var (result, user) = await _service.UpdateAsync(idPublic, request);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpDelete("{idPublic:guid}")]
    [Authorize(Policy = Permissions.UserDelete)]
    public async Task<IActionResult> Delete(Guid idPublic)
    {
        if (!await _service.DeleteAsync(idPublic))
            return NotFound();

        return NoContent();
    }
}
