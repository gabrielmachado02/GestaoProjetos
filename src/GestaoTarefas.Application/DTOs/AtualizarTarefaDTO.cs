using GestaoTarefas.Domain.Enums;

namespace GestaoTarefas.Application.DTOs;

public class AtualizarTarefaDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataVencimento { get; set; }
    public StatusTarefa Status { get; set; }
    public Guid UsuarioId { get; set; }
}