using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestaoTarefas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _tarefaService;

    public TarefasController(ITarefaService tarefaService)
    {
        _tarefaService = tarefaService;
    }

    [HttpGet("projeto/{projetoId}")]
    public async Task<ActionResult<IEnumerable<TarefaDTO>>> GetTarefasDoProjeto(Guid projetoId)
    {
        var tarefas = await _tarefaService.ObterTodasDoProjetoAsync(projetoId);
        return Ok(tarefas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TarefaDTO>> GetTarefa(Guid id)
    {
        var resultado = await _tarefaService.ObterPorIdAsync(id);
        
        if (!resultado.Sucesso)
            return NotFound(resultado.Mensagem);
            
        return Ok(resultado.Dados);
    }

    [HttpPost]
    public async Task<ActionResult<TarefaDTO>> CriarTarefa([FromBody] CriarTarefaDTO dto)
    {
        var resultado = await _tarefaService.CriarAsync(dto);
        
        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);
            
        return CreatedAtAction(
            nameof(GetTarefa), 
            new { id = resultado.Dados!.Id }, 
            resultado.Dados
        );
    }

    [HttpPut]
    public async Task<ActionResult<TarefaDTO>> AtualizarTarefa([FromBody] AtualizarTarefaDTO dto)
    {
        var resultado = await _tarefaService.AtualizarAsync(dto);
        
        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);
            
        return Ok(resultado.Dados);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoverTarefa(Guid id)
    {
        var resultado = await _tarefaService.RemoverAsync(id);
        
        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);
            
        return NoContent();
    }

    [HttpPost("comentario")]
    public async Task<ActionResult<TarefaDTO>> AdicionarComentario([FromBody] AdicionarComentarioDTO dto)
    {
        var resultado = await _tarefaService.AdicionarComentarioAsync(dto);
        
        if (!resultado.Sucesso)
            return BadRequest(resultado.Mensagem);
            
        return Ok(resultado.Dados);
    }
}