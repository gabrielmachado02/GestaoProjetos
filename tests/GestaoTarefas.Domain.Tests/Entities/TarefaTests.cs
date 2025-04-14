using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Enums;
using Xunit;

namespace GestaoTarefas.Domain.Tests.Entities;

public class TarefaTests
{
    [Fact]
    public void AtualizarStatus_QuandoStatusMuda_DeveAtualizarERegistrarAlteracao()
    {

        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);


        tarefa.AtualizarStatus(StatusTarefa.EmAndamento, usuarioId);


        Assert.Equal(StatusTarefa.EmAndamento, tarefa.Status);
        Assert.Single(tarefa.Alteracoes);
        Assert.Contains("Status alterado de", tarefa.Alteracoes.First().Descricao);
    }

    [Fact]
    public void AtualizarStatus_QuandoStatusIgual_NaoDeveAtualizarOuRegistrarAlteracao()
    {

        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);


        tarefa.AtualizarStatus(StatusTarefa.Pendente, usuarioId);


        Assert.Equal(StatusTarefa.Pendente, tarefa.Status);
        Assert.Empty(tarefa.Alteracoes);
    }

    [Fact]
    public void AtualizarDetalhes_QuandoDetalhesAtualizados_DeveAtualizarERegistrarAlteracao()
    {

        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        var novoTitulo = "Novo Título";
        var novaDescricao = "Nova Descrição";
        var novaData = DateTime.Now.AddDays(5);


        tarefa.AtualizarDetalhes(novoTitulo, novaDescricao, novaData, usuarioId);


        Assert.Equal(novoTitulo, tarefa.Titulo);
        Assert.Equal(novaDescricao, tarefa.Descricao);
        Assert.Equal(novaData, tarefa.DataVencimento);
        Assert.Single(tarefa.Alteracoes);
    }

    [Fact]
    public void AtualizarDetalhes_QuandoNadaMuda_NaoDeveRegistrarAlteracao()
    {

        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var titulo = "Tarefa Teste";
        var descricao = "Descrição da tarefa";
        var dataVencimento = DateTime.Now.AddDays(1).Date;
        var tarefa = new Tarefa(titulo, descricao, dataVencimento, PrioridadeTarefa.Media, projetoId);


        tarefa.AtualizarDetalhes(titulo, descricao, dataVencimento, usuarioId);


        Assert.Empty(tarefa.Alteracoes);
    }

    [Fact]
    public void AdicionarComentario_DeveAdicionarComentarioERegistrarAlteracao()
    {

        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição da tarefa", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        var conteudoComentario = "Este é um comentário de teste";


        tarefa.AdicionarComentario(conteudoComentario, usuarioId);


        Assert.Single(tarefa.Comentarios);
        Assert.Equal(conteudoComentario, tarefa.Comentarios.First().Conteudo);
        Assert.Single(tarefa.Alteracoes);
        Assert.Contains("Comentário adicionado", tarefa.Alteracoes.First().Descricao);
    }
}