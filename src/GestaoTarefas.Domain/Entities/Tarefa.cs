using GestaoTarefas.Domain.Enums;

namespace GestaoTarefas.Domain.Entities;

public class Tarefa
{
    private readonly List<AlteracaoTarefa> _alteracoes = new();
    private readonly List<Comentario> _comentarios = new();

    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public DateTime DataVencimento { get; private set; }
    public StatusTarefa Status { get; private set; }
    public PrioridadeTarefa Prioridade { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public Guid ProjetoId { get; private set; }
    public Projeto Projeto { get; private set; } = null!;
    public IReadOnlyCollection<AlteracaoTarefa> Alteracoes => _alteracoes.AsReadOnly();
    public IReadOnlyCollection<Comentario> Comentarios => _comentarios.AsReadOnly();

    private Tarefa() { }

    public Tarefa(string titulo, string descricao, DateTime dataVencimento, PrioridadeTarefa prioridade, Guid projetoId)
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        DataVencimento = dataVencimento;
        Status = StatusTarefa.Pendente;
        Prioridade = prioridade;
        DataCriacao = DateTime.UtcNow;
        ProjetoId = projetoId;
    }

    public void AtualizarStatus(StatusTarefa novoStatus, Guid usuarioId)
    {
        if (Status == novoStatus)
            return;

        var statusAntigo = Status;
        Status = novoStatus;
        
        RegistrarAlteracao($"Status alterado de {statusAntigo} para {novoStatus}", usuarioId);
    }

    public void AtualizarDetalhes(string titulo, string descricao, DateTime dataVencimento, Guid usuarioId)
    {
        var alteracoes = new List<string>();
        
        if (Titulo != titulo)
        {
            alteracoes.Add($"Título alterado de '{Titulo}' para '{titulo}'");
            Titulo = titulo;
        }
        
        if (Descricao != descricao)
        {
            alteracoes.Add($"Descrição atualizada");
            Descricao = descricao;
        }
        
        if (DataVencimento != dataVencimento)
        {
            alteracoes.Add($"Data de vencimento alterada de {DataVencimento:d} para {dataVencimento:d}");
            DataVencimento = dataVencimento;
        }
        
        if (alteracoes.Any())
        {
            RegistrarAlteracao(string.Join("; ", alteracoes), usuarioId);
        }
    }

    public void AdicionarComentario(string conteudo, Guid usuarioId)
    {
        var comentario = new Comentario(conteudo, Id, usuarioId);
        _comentarios.Add(comentario);
        
        RegistrarAlteracao($"Comentário adicionado: '{conteudo}'", usuarioId);
    }

    private void RegistrarAlteracao(string descricao, Guid usuarioId)
    {
        var alteracao = new AlteracaoTarefa(Id, descricao, usuarioId);
        _alteracoes.Add(alteracao);
    }
}