namespace GestaoTarefas.Domain.Entities;

public class Comentario
{
    public Guid Id { get; private set; }
    public string Conteudo { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public Guid TarefaId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Tarefa Tarefa { get; private set; } = null!;

    private Comentario() { }

    public Comentario(string conteudo, Guid tarefaId, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        Conteudo = conteudo;
        DataCriacao = DateTime.UtcNow;
        TarefaId = tarefaId;
        UsuarioId = usuarioId;
    }
}