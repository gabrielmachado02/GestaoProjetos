using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestaoTarefas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjetosController : ControllerBase
{
    private readonly IProjetoService _projetoService;

    public ProjetosController(IProjetoService projetoService)
    {
        _projetoService = projetoService;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<ProjetoDTO>>> GetProjetos(Guid usuarioId)
    {
        var projetos = await _projetoService.ObterTodosDoUsuarioAsync(usuarioId);
        return Ok(projetos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjetoDTO>> GetProjeto(Guid id)
    {
        var resultado = await _projetoService.ObterPorIdAsync(id);
        
        if (!resultado.Sucesso)
            return NotFound(resultado.Mensagem);
            
        return Ok(resultado.Dados);
    }

    [HttpPost]
    public async Task<ActionResult<ProjetoDTO>> CriarProjeto([FromBody] CriarProjetoDTO dto)
    {
        var resultado = await _projetoService.CriarAsync(dto);
        
        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);
            
        return CreatedAtAction(
            nameof(GetProjeto), 
            new { id = resultado.Dados!.Id }, 
            resultado.Dados
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoverProjeto(Guid id)
    {
        var resultado = await _projetoService.RemoverAsync(id);
        
        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);
            
        return NoContent();
    }
}