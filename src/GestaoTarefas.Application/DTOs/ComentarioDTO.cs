namespace GestaoTarefas.Application.DTOs;

public class ComentarioDTO
{
    public Guid Id { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public Guid UsuarioId { get; set; }
}