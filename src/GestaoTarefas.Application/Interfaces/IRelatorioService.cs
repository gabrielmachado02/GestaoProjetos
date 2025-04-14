namespace GestaoTarefas.Application.Interfaces;

public interface IRelatorioService
{
    Task<double> ObterMediaTarefasConcluidasPorUsuarioUltimos30DiasAsync();
}