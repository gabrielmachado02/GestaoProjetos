using GestaoTarefas.Application.DTOs;

namespace GestaoTarefas.Application.Interfaces;

public interface IProjetoService
{
    Task<IEnumerable<ProjetoDTO>> ObterTodosDoUsuarioAsync(Guid usuarioId);
    Task<ResultadoOperacao<ProjetoDTO>> ObterPorIdAsync(Guid id);
    Task<ResultadoOperacao<ProjetoDTO>> CriarAsync(CriarProjetoDTO dto);
    Task<ResultadoOperacao<bool>> RemoverAsync(Guid id);
}