using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Repositories;
using GestaoTarefas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoTarefas.Infrastructure.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly ApplicationDbContext _context;

    public TarefaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tarefa>> ObterTodasDoProjeto(Guid projetoId)
    {
        return await _context.Tarefas
            .Where(t => t.ProjetoId == projetoId)
            .ToListAsync();
    }

    public async Task<Tarefa?> ObterPorIdAsync(Guid id)
    {
        return await _context.Tarefas
            .Include(t => t.Alteracoes)
            .Include(t => t.Comentarios)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tarefa> AdicionarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
        return tarefa;
    }

    public async Task AtualizarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa != null)
        {
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> ContarTarefasDoProjetoAsync(Guid projetoId)
    {
        return await _context.Tarefas.CountAsync(t => t.ProjetoId == projetoId);
    }
}