using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Enums;
using Xunit;

namespace GestaoTarefas.Domain.Tests.Entities;

public class ProjetoTests
{
    [Fact]
    public void AdicionarTarefa_QuandoProjetoNaoAtingiuLimite_DeveAdicionarTarefa()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projeto.Id);

        // Act
        projeto.AdicionarTarefa(tarefa);

        // Assert
        Assert.Single(projeto.Tarefas);
        Assert.Contains(tarefa, projeto.Tarefas);
    }

    [Fact]
    public void AdicionarTarefa_QuandoProjetoAtingiuLimite_DeveLancarExcecao()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);

        for (int i = 0; i < 20; i++)
        {
            var tarefa = new Tarefa($"Tarefa {i}", $"Descrição {i}", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projeto.Id);
            projeto.AdicionarTarefa(tarefa);
        }

        var novaTarefa = new Tarefa("Tarefa Excedente", "Descrição", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projeto.Id);

        // Act & Assert
        var excecao = Assert.Throws<InvalidOperationException>(() => projeto.AdicionarTarefa(novaTarefa));
        Assert.Contains("não pode ter mais do que 20 tarefas", excecao.Message);
    }

    [Fact]
    public void RemoverTarefa_QuandoTarefaExiste_DeveRemoverTarefa()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projeto.Id);
        projeto.AdicionarTarefa(tarefa);

        // Act
        projeto.RemoverTarefa(tarefa.Id);

        // Assert
        Assert.Empty(projeto.Tarefas);
    }

    [Fact]
    public void RemoverTarefa_QuandoTarefaNaoExiste_DeveLancarExcecao()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);
        var tarefaInexistenteId = Guid.NewGuid();

        // Act & Assert
        var excecao = Assert.Throws<InvalidOperationException>(() => projeto.RemoverTarefa(tarefaInexistenteId));
        Assert.Equal("Tarefa não encontrada.", excecao.Message);
    }

    [Fact]
    public void PodeSemRemovido_QuandoNaoTemTarefasPendentes_DeveRetornarTrue()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);

        // Act & Assert
        Assert.True(projeto.PodeSemRemovido());
    }

    [Fact]
    public void PodeSemRemovido_QuandoTemTarefasPendentes_DeveRetornarFalse()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projeto.Id);
        projeto.AdicionarTarefa(tarefa);

        // Act & Assert
        Assert.False(projeto.PodeSemRemovido());
    }

    [Fact]
    public void ObterTarefa_QuandoTarefaExiste_DeveRetornarTarefa()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projeto.Id);
        projeto.AdicionarTarefa(tarefa);

        // Act
        var tarefaObtida = projeto.ObterTarefa(tarefa.Id);

        // Assert
        Assert.Equal(tarefa.Id, tarefaObtida.Id);
    }

    [Fact]
    public void ObterTarefa_QuandoTarefaNaoExiste_DeveLancarExcecao()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição do projeto", usuarioId);
        var tarefaInexistenteId = Guid.NewGuid();

        // Act & Assert
        var excecao = Assert.Throws<InvalidOperationException>(() => projeto.ObterTarefa(tarefaInexistenteId));
        Assert.Equal("Tarefa não encontrada.", excecao.Message);
    }
}