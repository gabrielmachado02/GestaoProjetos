namespace GestaoTarefas.Domain.Entities;

public class Usuario
{
    public Guid Id { get;  set; }
    public string Nome { get;  set; }
    public string Email { get;  set; }
    public ICollection<Projeto> Projetos { get;  set; }
    public bool EhGerente { get;  set; }

    private Usuario() { }

    public Usuario(string nome, string email, bool ehGerente = false)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        EhGerente = ehGerente;
        Projetos = new List<Projeto>();
    }
}