namespace GestaoTarefas.Application.DTOs;

public class CriarProjetoDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
}