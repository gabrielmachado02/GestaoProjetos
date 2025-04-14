namespace GestaoTarefas.Application.DTOs;

public class ProjetoDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public Guid UsuarioId { get; set; }
}