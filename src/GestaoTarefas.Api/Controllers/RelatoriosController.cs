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

    /// <summary>
    /// Obtém a média de tarefas concluídas por usuário nos últimos 30 dias
    /// </summary>
    /// <param name="request">Dados do solicitante incluindo seu ID e se é gerente</param>
    /// <returns>Média de tarefas concluídas por usuário nos últimos 30 dias</returns>
    /// <response code="200">Retorna a média de tarefas concluídas</response>
    /// <response code="403">Acesso negado para usuários que não são gerentes</response>
    /// <response code="400">Requisição inválida - parâmetros incorretos</response>
    [HttpPost("desempenho/tarefas-concluidas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> GetMediaTarefasConcluidasPorUsuario([FromBody] RelatorioRequestDTO request)
    {
        // Verifica se o usuário é gerente
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