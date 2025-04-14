using GestaoTarefas.Domain.Enums;

namespace GestaoTarefas.Application.DTOs;

public class CriarTarefaDTO
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataVencimento { get; set; }
    public PrioridadeTarefa Prioridade { get; set; }
    public Guid ProjetoId { get; set; }
}