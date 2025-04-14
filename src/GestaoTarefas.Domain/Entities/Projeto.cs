namespace GestaoTarefas.Domain.Entities;

public class Projeto
{
    private const int LimiteMaximoTarefas = 20;
    private readonly List<Tarefa> _tarefas = new();

    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    public IReadOnlyCollection<Tarefa> Tarefas => _tarefas.AsReadOnly();

    private Projeto() { }

    public Projeto(string nome, string descricao, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        DataCriacao = DateTime.UtcNow;
        UsuarioId = usuarioId;
    }

    public void AdicionarTarefa(Tarefa tarefa)
    {
        if (_tarefas.Count >= LimiteMaximoTarefas)
            throw new InvalidOperationException($"O projeto não pode ter mais do que {LimiteMaximoTarefas} tarefas.");

        _tarefas.Add(tarefa);
    }

    public void RemoverTarefa(Guid tarefaId)
    {
        var tarefa = _tarefas.FirstOrDefault(t => t.Id == tarefaId);
        
        if (tarefa == null)
            throw new InvalidOperationException("Tarefa não encontrada.");

        _tarefas.Remove(tarefa);
    }

    public bool PodeSemRemovido()
    {
        return !_tarefas.Any(t => t.Status != Enums.StatusTarefa.Concluida);
    }

    public Tarefa ObterTarefa(Guid id)
    {
        var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
        
        if (tarefa == null)
            throw new InvalidOperationException("Tarefa não encontrada.");
            
        return tarefa;
    }
}