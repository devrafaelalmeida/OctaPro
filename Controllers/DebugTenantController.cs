using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OctaPro.Data;
using OctaPro.DTO.Response;
using OctaPro.Tenancy;

namespace OctaPro.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/debug")]
public class DebugTenantController : ControllerBase
{
    private readonly ITenantContext _tenantContext;
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DebugTenantController> _logger;

    public DebugTenantController(
        ITenantContext tenantContext,
        AppDbContext appDbContext,
        ILogger<DebugTenantController> logger)
    {
        _tenantContext = tenantContext;
        _appDbContext = appDbContext;
        _logger = logger;
    }

    // TODO: remover este endpoint após validar a Fase 2
    [HttpGet("tenant-atual")]
    public ActionResult<TenantDto> GetTenantAtual()
    {
        if (_tenantContext.Current == null)
            return NotFound();

        return Ok(_tenantContext.Current);
    }

    // TODO: remover este endpoint após validar a Fase 3
    [HttpGet("teste-conexao-tenant")]
    public async Task<IActionResult> TesteConexaoTenant()
    {
        try
        {
            var canConnect = await _appDbContext.Database.CanConnectAsync();

            if (!canConnect)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Não foi possível conectar ao banco do tenant." });

            var user = await _appDbContext.Users
                .AsNoTracking()
                .OrderBy(user => user.Id)
                .Select(user => new
                {
                    user.Id,
                    user.IdPublic,
                    user.UserName,
                    user.Email,
                    user.EmailConfirmed,
                    user.CorporationId,
                    user.CreatedAt,
                    user.UpdatedAt
                })
                .FirstOrDefaultAsync();

            return Ok(new
            {
                banco = _tenantContext.Current?.Database,
                mensagem = "Conexão estabelecida com sucesso.",
                usuario = user
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Falha ao testar conexão com o banco do tenant.");

            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Não foi possível conectar ao banco do tenant." });
        }
    }
}
