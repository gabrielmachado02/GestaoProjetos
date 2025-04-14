using GestaoTarefas.Api.Controllers;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using GestaoTarefas.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GestaoTarefas.Api.Tests.Controllers;

public class TarefasControllerTests
{
    private readonly Mock<ITarefaService> _tarefaServiceMock;
    private readonly TarefasController _controller;

    public TarefasControllerTests()
    {
        _tarefaServiceMock = new Mock<ITarefaService>();
        _controller = new TarefasController(_tarefaServiceMock.Object);
    }

    [Fact]
    public async Task GetTarefasDoProjeto_DeveRetornarOk_ComListaDeTarefas()
    {

        var projetoId = Guid.NewGuid();
        var tarefas = new List<TarefaDTO>
        {
            new() { Id = Guid.NewGuid(), Titulo = "Tarefa 1", ProjetoId = projetoId },
            new() { Id = Guid.NewGuid(), Titulo = "Tarefa 2", ProjetoId = projetoId }
        };
        
        _tarefaServiceMock.Setup(s => s.ObterTodasDoProjetoAsync(projetoId))
            .ReturnsAsync(tarefas);


        var result = await _controller.GetTarefasDoProjeto(projetoId);


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTarefas = Assert.IsAssignableFrom<IEnumerable<TarefaDTO>>(okResult.Value);
        Assert.Equal(2, returnedTarefas.Count());
    }

    [Fact]
    public async Task GetTarefa_QuandoTarefaExiste_DeveRetornarOk_ComTarefa()
    {

        var tarefaId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var tarefa = new TarefaDTO { Id = tarefaId, Titulo = "Tarefa Teste", ProjetoId = projetoId };
        
        _tarefaServiceMock.Setup(s => s.ObterPorIdAsync(tarefaId))
            .ReturnsAsync(ResultadoOperacao<TarefaDTO>.Sucedido(tarefa));


        var result = await _controller.GetTarefa(tarefaId);


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTarefa = Assert.IsType<TarefaDTO>(okResult.Value);
        Assert.Equal(tarefaId, returnedTarefa.Id);
    }

    [Fact]
    public async Task GetTarefa_QuandoTarefaNaoExiste_DeveRetornarNotFound()
    {

        var tarefaId = Guid.NewGuid();
        
        _tarefaServiceMock.Setup(s => s.ObterPorIdAsync(tarefaId))
            .ReturnsAsync(ResultadoOperacao<TarefaDTO>.Falha("Tarefa não encontrada."));


        var result = await _controller.GetTarefa(tarefaId);


        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CriarTarefa_QuandoSucedido_DeveRetornarCreatedAtAction()
    {

        var dto = new CriarTarefaDTO
        {
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da nova tarefa",
            DataVencimento = DateTime.Now.AddDays(5),
            Prioridade = PrioridadeTarefa.Alta,
            ProjetoId = Guid.NewGuid()
        };
        
        var tarefaCriada = new TarefaDTO
        {
            Id = Guid.NewGuid(),
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataVencimento = dto.DataVencimento,
            Prioridade = dto.Prioridade,
            ProjetoId = dto.ProjetoId,
            Status = StatusTarefa.Pendente
        };
        
        _tarefaServiceMock.Setup(s => s.CriarAsync(dto))
            .ReturnsAsync(ResultadoOperacao<TarefaDTO>.Sucedido(tarefaCriada));


        var result = await _controller.CriarTarefa(dto);


        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedTarefa = Assert.IsType<TarefaDTO>(createdAtActionResult.Value);
        Assert.Equal(tarefaCriada.Id, returnedTarefa.Id);
        Assert.Equal("GetTarefa", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task CriarTarefa_QuandoFalhado_DeveRetornarBadRequest()
    {

        var dto = new CriarTarefaDTO
        {
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da nova tarefa",
            DataVencimento = DateTime.Now.AddDays(5),
            Prioridade = PrioridadeTarefa.Alta,
            ProjetoId = Guid.NewGuid()
        };
        
        _tarefaServiceMock.Setup(s => s.CriarAsync(dto))
            .ReturnsAsync(ResultadoOperacao<TarefaDTO>.Falha("Erro ao criar tarefa."));


        var result = await _controller.CriarTarefa(dto);


        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task AtualizarTarefa_QuandoSucedido_DeveRetornarOk()
    {

        var dto = new AtualizarTarefaDTO
        {
            Id = Guid.NewGuid(),
            Titulo = "Tarefa Atualizada",
            Descricao = "Descrição atualizada",
            DataVencimento = DateTime.Now.AddDays(7),
            Status = StatusTarefa.EmAndamento,
            UsuarioId = Guid.NewGuid()
        };
        
        var tarefaAtualizada = new TarefaDTO
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataVencimento = dto.DataVencimento,
            Status = dto.Status,
            ProjetoId = Guid.NewGuid()
        };
        
        _tarefaServiceMock.Setup(s => s.AtualizarAsync(dto))
            .ReturnsAsync(ResultadoOperacao<TarefaDTO>.Sucedido(tarefaAtualizada));


        var result = await _controller.AtualizarTarefa(dto);


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTarefa = Assert.IsType<TarefaDTO>(okResult.Value);
        Assert.Equal(dto.Id, returnedTarefa.Id);
        Assert.Equal(dto.Titulo, returnedTarefa.Titulo);
    }

    [Fact]
    public async Task AtualizarTarefa_QuandoFalhado_DeveRetornarBadRequest()
    {

        var dto = new AtualizarTarefaDTO
        {
            Id = Guid.NewGuid(),
            Titulo = "Tarefa Atualizada",
            Descricao = "Descrição atualizada",
            DataVencimento = DateTime.Now.AddDays(7),
            Status = StatusTarefa.EmAndamento,
            UsuarioId = Guid.NewGuid()
        };
        
        _tarefaServiceMock.Setup(s => s.AtualizarAsync(dto))
            .ReturnsAsync(ResultadoOperacao<TarefaDTO>.Falha("Tarefa não encontrada."));


        var result = await _controller.AtualizarTarefa(dto);


        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task RemoverTarefa_QuandoSucedido_DeveRetornarNoContent()
    {

        var tarefaId = Guid.NewGuid();
        
        _tarefaServiceMock.Setup(s => s.RemoverAsync(tarefaId))
            .ReturnsAsync(ResultadoOperacao<bool>.Sucedido(true));


        var result = await _controller.RemoverTarefa(tarefaId);


        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AdicionarComentario_QuandoSucedido_DeveRetornarOk()
    {

        var dto = new AdicionarComentarioDTO
        {
            TarefaId = Guid.NewGuid(),
            Conteudo = "Este é um comentário de teste",
            UsuarioId = Guid.NewGuid()
        };
        
        var tarefaComComentario = new TarefaDTO
        {
            Id = dto.TarefaId,
            Titulo = "Tarefa com Comentário",
            Comentarios = new List<ComentarioDTO>
            {
                new() 
                { 
                    Id = Guid.NewGuid(),
                    Conteudo = dto.Conteudo,
                    UsuarioId = dto.UsuarioId,
                    DataCriacao = DateTime.UtcNow
                }
            }
        };
        
        _tarefaServiceMock.Setup(s => s.AdicionarComentarioAsync(dto))
            .ReturnsAsync(ResultadoOperacao<TarefaDTO>.Sucedido(tarefaComComentario));


        var result = await _controller.AdicionarComentario(dto);


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTarefa = Assert.IsType<TarefaDTO>(okResult.Value);
        Assert.Equal(dto.TarefaId, returnedTarefa.Id);
        Assert.Single(returnedTarefa.Comentarios);
        Assert.Equal(dto.Conteudo, returnedTarefa.Comentarios.First().Conteudo);
    }
}