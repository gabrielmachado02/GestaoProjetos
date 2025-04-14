using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Enums;
using GestaoTarefas.Infrastructure.Data;
using GestaoTarefas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoTarefas.Infrastructure.Tests.Repositories;

public class TarefaRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task ObterTodasDoProjeto_DeveRetornarTarefasDoProjeto()
    {

        var options = CreateNewContextOptions();
        var usuarioId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var outroprojetoId = Guid.NewGuid();


        using (var context = new ApplicationDbContext(options))
        {
            var projeto = new Projeto("Projeto Teste", "Descrição", usuarioId);
            projeto.GetType().GetProperty("Id")?.SetValue(projeto, projetoId);
            
            var outroProjeto = new Projeto("Outro Projeto", "Descrição", usuarioId);
            outroProjeto.GetType().GetProperty("Id")?.SetValue(outroProjeto, outroprojetoId);
            
            context.Projetos.Add(projeto);
            context.Projetos.Add(outroProjeto);
            
            var tarefa1 = new Tarefa("Tarefa 1", "Descrição 1", DateTime.Now.AddDays(1), PrioridadeTarefa.Baixa, projetoId);
            var tarefa2 = new Tarefa("Tarefa 2", "Descrição 2", DateTime.Now.AddDays(2), PrioridadeTarefa.Media, projetoId);
            var tarefa3 = new Tarefa("Tarefa 3", "Descrição 3", DateTime.Now.AddDays(3), PrioridadeTarefa.Alta, outroprojetoId);
            
            context.Tarefas.Add(tarefa1);
            context.Tarefas.Add(tarefa2);
            context.Tarefas.Add(tarefa3);
            
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new TarefaRepository(context);
            var tarefas = await repository.ObterTodasDoProjeto(projetoId);


            Assert.Equal(2, tarefas.Count());
            Assert.All(tarefas, t => Assert.Equal(projetoId, t.ProjetoId));
        }
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoTarefaExiste_DeveRetornarTarefa()
    {

        var options = CreateNewContextOptions();
        var projetoId = Guid.NewGuid();
        var tarefaId = Guid.NewGuid();
        

        using (var context = new ApplicationDbContext(options))
        {
            var tarefa = new Tarefa("Tarefa Teste", "Descrição", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
            tarefa.GetType().GetProperty("Id")?.SetValue(tarefa, tarefaId);
            
            context.Tarefas.Add(tarefa);
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new TarefaRepository(context);
            var tarefaObtida = await repository.ObterPorIdAsync(tarefaId);


            Assert.NotNull(tarefaObtida);
            Assert.Equal(tarefaId, tarefaObtida!.Id);
            Assert.Equal("Tarefa Teste", tarefaObtida.Titulo);
        }
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarERetornarTarefa()
    {

        var options = CreateNewContextOptions();
        var projetoId = Guid.NewGuid();
        var tarefa = new Tarefa("Nova Tarefa", "Descrição da nova tarefa", DateTime.Now.AddDays(5), PrioridadeTarefa.Alta, projetoId);


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new TarefaRepository(context);
            var tarefaAdicionada = await repository.AdicionarAsync(tarefa);


            Assert.NotNull(tarefaAdicionada);
            Assert.Equal(tarefa.Id, tarefaAdicionada.Id);
            Assert.Equal("Nova Tarefa", tarefaAdicionada.Titulo);
            

            var tarefaSalva = await context.Tarefas.FindAsync(tarefa.Id);
            Assert.NotNull(tarefaSalva);
            Assert.Equal("Nova Tarefa", tarefaSalva!.Titulo);
        }
    }

    [Fact]
    public async Task RemoverAsync_QuandoTarefaExiste_DeveRemoverTarefa()
    {

        var options = CreateNewContextOptions();
        var projetoId = Guid.NewGuid();
        var tarefaId = Guid.NewGuid();
        

        using (var context = new ApplicationDbContext(options))
        {
            var tarefa = new Tarefa("Tarefa para Remover", "Descrição", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
            tarefa.GetType().GetProperty("Id")?.SetValue(tarefa, tarefaId);
            
            context.Tarefas.Add(tarefa);
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new TarefaRepository(context);
            await repository.RemoverAsync(tarefaId);
            

            var tarefaRemovida = await context.Tarefas.FindAsync(tarefaId);
            Assert.Null(tarefaRemovida);
        }
    }

    [Fact]
    public async Task ContarTarefasDoProjetoAsync_DeveRetornarQuantidadeCorreta()
    {

        var options = CreateNewContextOptions();
        var usuarioId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        

        using (var context = new ApplicationDbContext(options))
        {
            var projeto = new Projeto("Projeto Teste", "Descrição", usuarioId);
            projeto.GetType().GetProperty("Id")?.SetValue(projeto, projetoId);
            
            context.Projetos.Add(projeto);
            
            for (int i = 0; i < 5; i++)
            {
                var tarefa = new Tarefa($"Tarefa {i}", $"Descrição {i}", DateTime.Now.AddDays(i), PrioridadeTarefa.Media, projetoId);
                context.Tarefas.Add(tarefa);
            }
            
            await context.SaveChangesAsync();
        }


        using (var context = new ApplicationDbContext(options))
        {
            var repository = new TarefaRepository(context);
            var quantidade = await repository.ContarTarefasDoProjetoAsync(projetoId);


            Assert.Equal(5, quantidade);
        }
    }
}