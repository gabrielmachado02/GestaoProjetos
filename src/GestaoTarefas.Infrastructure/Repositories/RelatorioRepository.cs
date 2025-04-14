using GestaoTarefas.Domain.Enums;
using GestaoTarefas.Domain.Repositories;
using GestaoTarefas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoTarefas.Infrastructure.Repositories;

public class RelatorioRepository : IRelatorioRepository
{
    private readonly ApplicationDbContext _context;

    public RelatorioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<double> ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync()
    {
        var dataInicio = DateTime.UtcNow.AddDays(-30);
        

        var tarefasConcluidas = await _context.Tarefas
            .Where(t => t.Status == StatusTarefa.Concluida)
            .Where(t => t.Alteracoes.Any(a => 
                a.Descricao.Contains("Status alterado de") && 
                a.Descricao.Contains("para Concluida") && 
                a.DataAlteracao >= dataInicio))
            .ToListAsync();
            

        var usuariosCount = await _context.Usuarios.CountAsync();
        
        if (usuariosCount == 0)
            return 0;
            
        return (double)tarefasConcluidas.Count / usuariosCount;
    }
}