using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Infrastructure.Data;
using GestaoTarefas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoTarefas.Infrastructure.Tests.Repositories;

public class ProjetoRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task ObterTodosDoUsuarioAsync_DeveRetornarProjetosDoUsuario()
    {

        var options = CreateNewContextOptions();
        var usuarioId = Guid.NewGuid();
        var outroUsuarioId = Guid.NewGuid();


        using (var context = new ApplicationDbContext(options))
        {
            var usuario = new Usuario("Teste", "teste@email.com");
            usuario.GetType().GetProperty("Id")?.SetValue(usuario, usuarioId);
            
            context.Usuarios.Add(usuario);
            
            context.Projetos.Add(new Projeto("Projeto 1", "Descrição 1", usuarioId));
            context.Projetos.Add(new Projeto("Projeto 2", "Descrição 2", usuarioId));
            context.Projetos.Add(new Projeto("Projeto 3", "Descrição 3", outroUsuarioId)); // Projeto de outro usuário
            
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new ProjetoRepository(context);
            var projetos = await repository.ObterTodosDoUsuarioAsync(usuarioId);


            Assert.Equal(2, projetos.Count());
            Assert.All(projetos, p => Assert.Equal(usuarioId, p.UsuarioId));
        }
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoProjetoExiste_DeveRetornarProjeto()
    {

        var options = CreateNewContextOptions();
        var usuarioId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        

        using (var context = new ApplicationDbContext(options))
        {
            var projeto = new Projeto("Projeto Teste", "Descrição", usuarioId);
            projeto.GetType().GetProperty("Id")?.SetValue(projeto, projetoId);
            
            context.Projetos.Add(projeto);
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new ProjetoRepository(context);
            var projetoObtido = await repository.ObterPorIdAsync(projetoId);


            Assert.NotNull(projetoObtido);
            Assert.Equal(projetoId, projetoObtido!.Id);
            Assert.Equal("Projeto Teste", projetoObtido.Nome);
        }
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarERetornarProjeto()
    {

        var options = CreateNewContextOptions();
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Novo Projeto", "Descrição do novo projeto", usuarioId);


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new ProjetoRepository(context);
            var projetoAdicionado = await repository.AdicionarAsync(projeto);


            Assert.NotNull(projetoAdicionado);
            Assert.Equal(projeto.Id, projetoAdicionado.Id);
            Assert.Equal("Novo Projeto", projetoAdicionado.Nome);
            

            var projetoSalvo = await context.Projetos.FindAsync(projeto.Id);
            Assert.NotNull(projetoSalvo);
            Assert.Equal("Novo Projeto", projetoSalvo!.Nome);
        }
    }

    [Fact]
    public async Task RemoverAsync_QuandoProjetoExiste_DeveRemoverProjeto()
    {

        var options = CreateNewContextOptions();
        var usuarioId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        

        using (var context = new ApplicationDbContext(options))
        {
            var projeto = new Projeto("Projeto para Remover", "Descrição", usuarioId);
            projeto.GetType().GetProperty("Id")?.SetValue(projeto, projetoId);
            
            context.Projetos.Add(projeto);
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new ProjetoRepository(context);
            await repository.RemoverAsync(projetoId);
            

            var projetoRemovido = await context.Projetos.FindAsync(projetoId);
            Assert.Null(projetoRemovido);
        }
    }

    [Fact]
    public async Task UsuarioExisteAsync_QuandoUsuarioExiste_DeveRetornarTrue()
    {

        var options = CreateNewContextOptions();
        var usuarioId = Guid.NewGuid();
        

        using (var context = new ApplicationDbContext(options))
        {
            var usuario = new Usuario("Teste", "teste@email.com");
            usuario.GetType().GetProperty("Id")?.SetValue(usuario, usuarioId);
            
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new ProjetoRepository(context);
            var resultado = await repository.UsuarioExisteAsync(usuarioId);


            Assert.True(resultado);
        }
    }
}