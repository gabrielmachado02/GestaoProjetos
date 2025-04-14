using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Enums;
using Xunit;

namespace GestaoTarefas.Domain.Tests.Entities;

public class TarefaTests
{
    [Fact]
    public void AtualizarStatus_QuandoStatusMuda_DeveAtualizarERegistrarAlteracao()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);

        // Act
        tarefa.AtualizarStatus(StatusTarefa.EmAndamento, usuarioId);

        // Assert
        Assert.Equal(StatusTarefa.EmAndamento, tarefa.Status);
        Assert.Single(tarefa.Alteracoes);
        Assert.Contains("Status alterado de", tarefa.Alteracoes.First().Descricao);
    }

    [Fact]
    public void AtualizarStatus_QuandoStatusIgual_NaoDeveAtualizarOuRegistrarAlteracao()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);

        // Act
        tarefa.AtualizarStatus(StatusTarefa.Pendente, usuarioId);

        // Assert
        Assert.Equal(StatusTarefa.Pendente, tarefa.Status);
        Assert.Empty(tarefa.Alteracoes);
    }

    [Fact]
    public void AtualizarDetalhes_QuandoDetalhesAtualizados_DeveAtualizarERegistrarAlteracao()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        var novoTitulo = "Novo Título";
        var novaDescricao = "Nova Descrição";
        var novaData = DateTime.Now.AddDays(5);

        // Act
        tarefa.AtualizarDetalhes(novoTitulo, novaDescricao, novaData, usuarioId);

        // Assert
        Assert.Equal(novoTitulo, tarefa.Titulo);
        Assert.Equal(novaDescricao, tarefa.Descricao);
        Assert.Equal(novaData, tarefa.DataVencimento);
        Assert.Single(tarefa.Alteracoes);
    }

    [Fact]
    public void AtualizarDetalhes_QuandoNadaMuda_NaoDeveRegistrarAlteracao()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var titulo = "Tarefa Teste";
        var descricao = "Descrição da tarefa";
        var dataVencimento = DateTime.Now.AddDays(1).Date; // Removendo componente de hora para comparação
        var tarefa = new Tarefa(titulo, descricao, dataVencimento, PrioridadeTarefa.Media, projetoId);

        // Act
        tarefa.AtualizarDetalhes(titulo, descricao, dataVencimento, usuarioId);

        // Assert
        Assert.Empty(tarefa.Alteracoes);
    }

    [Fact]
    public void AdicionarComentario_DeveAdicionarComentarioERegistrarAlteracao()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        var conteudoComentario = "Este é um comentário de teste";

        // Act
        tarefa.AdicionarComentario(conteudoComentario, usuarioId);

        // Assert
        Assert.Single(tarefa.Comentarios);
        Assert.Equal(conteudoComentario, tarefa.Comentarios.First().Conteudo);
        Assert.Single(tarefa.Alteracoes);
        Assert.Contains("Comentário adicionado", tarefa.Alteracoes.First().Descricao);
    }
}