namespace GestaoTarefas.Application.DTOs;

public class AdicionarComentarioDTO
{
    public Guid TarefaId { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
}