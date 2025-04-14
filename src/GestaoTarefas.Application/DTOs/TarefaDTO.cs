using GestaoTarefas.Domain.Enums;

namespace GestaoTarefas.Application.DTOs;

public class TarefaDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataVencimento { get; set; }
    public StatusTarefa Status { get; set; }
    public PrioridadeTarefa Prioridade { get; set; }
    public DateTime DataCriacao { get; set; }
    public Guid ProjetoId { get; set; }
    public List<AlteracaoTarefaDTO> Alteracoes { get; set; } = new();
    public List<ComentarioDTO> Comentarios { get; set; } = new();
}