namespace GestaoTarefas.Domain.Entities;

public class AlteracaoTarefa
{
    public Guid Id { get; private set; }
    public Guid TarefaId { get; private set; }
    public string Descricao { get; private set; }
    public DateTime DataAlteracao { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Tarefa Tarefa { get; private set; } = null!;

    private AlteracaoTarefa() { }

    public AlteracaoTarefa(Guid tarefaId, string descricao, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        TarefaId = tarefaId;
        Descricao = descricao;
        DataAlteracao = DateTime.UtcNow;
        UsuarioId = usuarioId;
    }
}