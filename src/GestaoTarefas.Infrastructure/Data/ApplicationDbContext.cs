using GestaoTarefas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoTarefas.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Projeto> Projetos { get; set; } = null!;
    public DbSet<Tarefa> Tarefas { get; set; } = null!;
    public DbSet<AlteracaoTarefa> AlteracoesTarefas { get; set; } = null!;
    public DbSet<Comentario> Comentarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Projetos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Projeto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.HasMany(p => p.Tarefas)
                .WithOne(t => t.Projeto)
                .HasForeignKey(t => t.ProjetoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.HasMany(t => t.Alteracoes)
                .WithOne(a => a.Tarefa)
                .HasForeignKey(a => a.TarefaId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(t => t.Comentarios)
                .WithOne(c => c.Tarefa)
                .HasForeignKey(c => c.TarefaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AlteracaoTarefa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Conteudo).IsRequired().HasMaxLength(500);
        });

        // Seed de usuário para testes
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario("Usuário Teste", "usuario@teste.com", false) { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            new Usuario("Gerente Teste", "gerente@teste.com", true) { Id = Guid.Parse("22222222-2222-2222-2222-222222222222") }
        );
    }
}