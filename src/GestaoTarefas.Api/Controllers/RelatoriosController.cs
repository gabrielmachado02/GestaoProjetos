using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestaoTarefas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpPost("desempenho/tarefas-concluidas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> GetMediaTarefasConcluidasPorUsuario([FromBody] RelatorioRequestDTO request)
    {
        if (!request.EhGerente)
        {
            return Forbid("Apenas gerentes podem acessar relatórios de desempenho.");
        }
        
        var media = await _relatorioService.ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync();
        
        return Ok(new { 
            MediaTarefasConcluidasPorUsuario = media,
            Periodo = "Últimos 30 dias",
            UsuarioId = request.UsuarioId
        });
    }
}