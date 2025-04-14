namespace GestaoTarefas.Application.DTOs;

public class RelatorioRequestDTO
{
    public bool EhGerente { get; set; }
    public Guid UsuarioId { get; set; }
}