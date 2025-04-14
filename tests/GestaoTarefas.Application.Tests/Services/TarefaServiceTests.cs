using AutoMapper;
using GestaoTarefas.Application.DTOs;
using GestaoTarefas.Application.Interfaces;
using GestaoTarefas.Application.Mappings;
using GestaoTarefas.Application.Services;
using GestaoTarefas.Domain.Entities;
using GestaoTarefas.Domain.Enums;
using GestaoTarefas.Domain.Repositories;
using Moq;
using Xunit;

namespace GestaoTarefas.Application.Tests.Services;

public class TarefaServiceTests
{
    private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
    private readonly Mock<IProjetoRepository> _projetoRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ITarefaService _tarefaService;

    public TarefaServiceTests()
    {
        _tarefaRepositoryMock = new Mock<ITarefaRepository>();
        _projetoRepositoryMock = new Mock<IProjetoRepository>();
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        _mapper = mapperConfig.CreateMapper();
        
        _tarefaService = new TarefaService(_tarefaRepositoryMock.Object, _projetoRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task ObterTodasDoProjetoAsync_DeveRetornarListaDeTarefas()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var tarefas = new List<Tarefa>
        {
            new Tarefa("Tarefa 1", "Descrição 1", DateTime.Now.AddDays(1), PrioridadeTarefa.Baixa, projetoId),
            new Tarefa("Tarefa 2", "Descrição 2", DateTime.Now.AddDays(2), PrioridadeTarefa.Media, projetoId)
        };
        
        _tarefaRepositoryMock.Setup(r => r.ObterTodasDoProjeto(projetoId))
            .ReturnsAsync(tarefas);

        // Act
        var result = await _tarefaService.ObterTodasDoProjetoAsync(projetoId);

        // Assert
        Assert.Equal(2, result.Count());
        _tarefaRepositoryMock.Verify(r => r.ObterTodasDoProjeto(projetoId), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoTarefaExiste_DeveRetornarTarefa()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        
        _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(tarefaId))
            .ReturnsAsync(tarefa);

        // Act
        var result = await _tarefaService.ObterPorIdAsync(tarefaId);

        // Assert
        Assert.True(result.Sucesso);
        Assert.NotNull(result.Dados);
        Assert.Equal(tarefa.Titulo, result.Dados.Titulo);
        _tarefaRepositoryMock.Verify(r => r.ObterPorIdAsync(tarefaId), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoTarefaNaoExiste_DeveRetornarFalha()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        
        _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(tarefaId))
            .ReturnsAsync((Tarefa)null);

        // Act
        var result = await _tarefaService.ObterPorIdAsync(tarefaId);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Null(result.Dados);
        Assert.Equal("Tarefa não encontrada.", result.Mensagem);
        _tarefaRepositoryMock.Verify(r => r.ObterPorIdAsync(tarefaId), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_QuandoProjetoExisteELimiteNaoExcedido_DeveCriarTarefa()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição", usuarioId);
        
        var dto = new CriarTarefaDTO
        {
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da nova tarefa",
            DataVencimento = DateTime.Now.AddDays(5),
            Prioridade = PrioridadeTarefa.Alta,
            ProjetoId = projetoId
        };
        
        _projetoRepositoryMock.Setup(r => r.ObterPorIdAsync(projetoId))
            .ReturnsAsync(projeto);
        
        _tarefaRepositoryMock.Setup(r => r.ContarTarefasDoProjetoAsync(projetoId))
            .ReturnsAsync(10); // Abaixo do limite de 20
        
        _tarefaRepositoryMock.Setup(r => r.AdicionarAsync(It.IsAny<Tarefa>()))
            .ReturnsAsync((Tarefa t) => t);

        // Act
        var result = await _tarefaService.CriarAsync(dto);

        // Assert
        Assert.True(result.Sucesso);
        Assert.NotNull(result.Dados);
        Assert.Equal(dto.Titulo, result.Dados.Titulo);
        Assert.Equal(dto.Descricao, result.Dados.Descricao);
        Assert.Equal(dto.Prioridade, result.Dados.Prioridade);
        Assert.Equal(StatusTarefa.Pendente, result.Dados.Status);
        _projetoRepositoryMock.Verify(r => r.ObterPorIdAsync(projetoId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.ContarTarefasDoProjetoAsync(projetoId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Tarefa>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_QuandoProjetoNaoExiste_DeveRetornarFalha()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        
        var dto = new CriarTarefaDTO
        {
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da nova tarefa",
            DataVencimento = DateTime.Now.AddDays(5),
            Prioridade = PrioridadeTarefa.Alta,
            ProjetoId = projetoId
        };
        
        _projetoRepositoryMock.Setup(r => r.ObterPorIdAsync(projetoId))
            .ReturnsAsync((Projeto)null);

        // Act
        var result = await _tarefaService.CriarAsync(dto);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Null(result.Dados);
        Assert.Equal("Projeto não encontrado.", result.Mensagem);
        _projetoRepositoryMock.Verify(r => r.ObterPorIdAsync(projetoId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Tarefa>()), Times.Never);
    }

    [Fact]
    public async Task CriarAsync_QuandoLimiteExcedido_DeveRetornarFalha()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var projeto = new Projeto("Projeto Teste", "Descrição", usuarioId);
        
        var dto = new CriarTarefaDTO
        {
            Titulo = "Nova Tarefa",
            Descricao = "Descrição da nova tarefa",
            DataVencimento = DateTime.Now.AddDays(5),
            Prioridade = PrioridadeTarefa.Alta,
            ProjetoId = projetoId
        };
        
        _projetoRepositoryMock.Setup(r => r.ObterPorIdAsync(projetoId))
            .ReturnsAsync(projeto);
        
        _tarefaRepositoryMock.Setup(r => r.ContarTarefasDoProjetoAsync(projetoId))
            .ReturnsAsync(20); // No limite

        // Act
        var result = await _tarefaService.CriarAsync(dto);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Null(result.Dados);
        Assert.Contains("limite máximo", result.Mensagem);
        _projetoRepositoryMock.Verify(r => r.ObterPorIdAsync(projetoId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.ContarTarefasDoProjetoAsync(projetoId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Tarefa>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarAsync_QuandoTarefaExiste_DeveAtualizarTarefa()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        
        var dto = new AtualizarTarefaDTO
        {
            Id = tarefaId,
            Titulo = "Tarefa Atualizada",
            Descricao = "Descrição atualizada",
            DataVencimento = DateTime.Now.AddDays(7),
            Status = StatusTarefa.EmAndamento,
            UsuarioId = usuarioId
        };
        
        _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(tarefaId))
            .ReturnsAsync(tarefa);
        
        _tarefaRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<Tarefa>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _tarefaService.AtualizarAsync(dto);

        // Assert
        Assert.True(result.Sucesso);
        Assert.NotNull(result.Dados);
        _tarefaRepositoryMock.Verify(r => r.ObterPorIdAsync(tarefaId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.AtualizarAsync(It.IsAny<Tarefa>()), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_QuandoTarefaExiste_DeveRemoverTarefa()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        
        _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(tarefaId))
            .ReturnsAsync(tarefa);
        
        _tarefaRepositoryMock.Setup(r => r.RemoverAsync(tarefaId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _tarefaService.RemoverAsync(tarefaId);

        // Assert
        Assert.True(result.Sucesso);
        Assert.True(result.Dados);
        _tarefaRepositoryMock.Verify(r => r.ObterPorIdAsync(tarefaId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.RemoverAsync(tarefaId), Times.Once);
    }

    [Fact]
    public async Task AdicionarComentarioAsync_QuandoTarefaExiste_DeveAdicionarComentario()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var tarefa = new Tarefa("Tarefa Teste", "Descrição", DateTime.Now.AddDays(1), PrioridadeTarefa.Media, projetoId);
        
        var dto = new AdicionarComentarioDTO
        {
            TarefaId = tarefaId,
            Conteudo = "Este é um comentário de teste",
            UsuarioId = usuarioId
        };
        
        _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(tarefaId))
            .ReturnsAsync(tarefa);
        
        _tarefaRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<Tarefa>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _tarefaService.AdicionarComentarioAsync(dto);

        // Assert
        Assert.True(result.Sucesso);
        Assert.NotNull(result.Dados);
        _tarefaRepositoryMock.Verify(r => r.ObterPorIdAsync(tarefaId), Times.Once);
        _tarefaRepositoryMock.Verify(r => r.AtualizarAsync(It.IsAny<Tarefa>()), Times.Once);
    }
}