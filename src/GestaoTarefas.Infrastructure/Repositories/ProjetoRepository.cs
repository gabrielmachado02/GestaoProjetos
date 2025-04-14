using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Repositories;
using GestaoTarefas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoTarefas.Infrastructure.Repositories;

public class ProjetoRepository : IProjetoRepository
{
    private readonly ApplicationDbContext _context;

    public ProjetoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Projeto>> ObterTodosDoUsuarioAsync(Guid usuarioId)
    {
        return await _context.Projetos
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<Projeto?> ObterPorIdAsync(Guid id)
    {
        return await _context.Projetos
            .Include(p => p.Tarefas)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Projeto> AdicionarAsync(Projeto projeto)
    {
        _context.Projetos.Add(projeto);
        await _context.SaveChangesAsync();
        return projeto;
    }

    public async Task AtualizarAsync(Projeto projeto)
    {
        _context.Projetos.Update(projeto);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var projeto = await _context.Projetos.FindAsync(id);
        if (projeto != null)
        {
            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> UsuarioExisteAsync(Guid usuarioId)
    {
        return await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
    }
}