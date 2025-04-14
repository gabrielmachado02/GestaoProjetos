namespace GestaoTarefas.Domain.Repositories;

public interface IRelatorioRepository
{
    Task<double> ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync();
}