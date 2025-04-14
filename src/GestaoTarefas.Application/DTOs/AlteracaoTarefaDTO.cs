namespace GestaoTarefas.Application.DTOs;

public class AlteracaoTarefaDTO
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataAlteracao { get; set; }
    public Guid UsuarioId { get; set; }
}