using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Domain.Repositories;

public interface IProjetoRepository
{
    Task<IEnumerable<Projeto>> ObterTodosDoUsuarioAsync(Guid usuarioId);
    Task<Projeto?> ObterPorIdAsync(Guid id);
    Task<Projeto> AdicionarAsync(Projeto projeto);
    Task AtualizarAsync(Projeto projeto);
    Task RemoverAsync(Guid id);
    Task<bool> UsuarioExisteAsync(Guid usuarioId);
}