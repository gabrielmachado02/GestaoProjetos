using GestaoTarefas.Application.Interfaces;
using GestaoTarefas.Domain.Repositories;

namespace GestaoTarefas.Application.Services;

public class RelatorioService : IRelatorioService
{
    private readonly IRelatorioRepository _relatorioRepository;

    public RelatorioService(IRelatorioRepository relatorioRepository)
    {
        _relatorioRepository = relatorioRepository;
    }

    public async Task<double> ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync()
    {
        return await _relatorioRepository.ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync();
    }
}