using GestaoTarefas.Domain.Entities;

namespace GestaoTarefas.Domain.Repositories;

public interface ITarefaRepository
{
    Task<IEnumerable<Tarefa>> ObterTodasDoProjeto(Guid projetoId);
    Task<Tarefa?> ObterPorIdAsync(Guid id);
    Task<Tarefa> AdicionarAsync(Tarefa tarefa);
    Task AtualizarAsync(Tarefa tarefa);
    Task RemoverAsync(Guid id);
    Task<int> ContarTarefasDoProjetoAsync(Guid projetoId);
}