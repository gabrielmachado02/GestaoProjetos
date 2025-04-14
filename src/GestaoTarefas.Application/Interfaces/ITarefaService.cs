using GestaoTarefas.Application.DTOs;

namespace GestaoTarefas.Application.Interfaces;

public interface ITarefaService
{
    Task<IEnumerable<TarefaDTO>> ObterTodasDoProjetoAsync(Guid projetoId);
    Task<ResultadoOperacao<TarefaDTO>> ObterPorIdAsync(Guid id);
    Task<ResultadoOperacao<TarefaDTO>> CriarAsync(CriarTarefaDTO dto);
    Task<ResultadoOperacao<TarefaDTO>> AtualizarAsync(AtualizarTarefaDTO dto);
    Task<ResultadoOperacao<bool>> RemoverAsync(Guid id);
    Task<ResultadoOperacao<TarefaDTO>> AdicionarComentarioAsync(AdicionarComentarioDTO dto);
}